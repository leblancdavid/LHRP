using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.LiquidTransfers;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Resources;
using System;
using System.Collections.Generic;

namespace LHRP.Api.Protocol.Pipetting
{
    public class LiquidToManyAspirate : IPipettingCommand
    {
        public Guid CommandId { get; private set; }
        private AspirateParameters _parameters;
        public ChannelPattern<LiquidToManyTransfer> TransferGroup { get; private set; }
        public double AdditionalAspirateVolume { get; set; }
        public int RetryCount { get; private set; }
        public ResourcesUsage ResourcesUsed { get; private set; }

        public LiquidToManyAspirate(AspirateParameters parameters,
            ChannelPattern<LiquidToManyTransfer> transferGroup,
            double additionalAspirateVolume,
            int retryAttempt = 0)
        {
            _parameters = parameters;
            TransferGroup = transferGroup;
            AdditionalAspirateVolume = additionalAspirateVolume;
            CommandId = Guid.NewGuid();
            RetryCount = retryAttempt;
            ResourcesUsed = new ResourcesUsage();
            foreach (var target in TransferGroup.GetActiveChannels())
            {
                ResourcesUsed.AddConsumableLiquidUsage(target.Source, target.GetTotalTransferVolume());
            }
        }


        public void ApplyChannelMask(ChannelPattern channelPattern)
        {
            TransferGroup.Mask(channelPattern);
        }

        public Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            return Result.Ok<IEnumerable<IRunnableCommand>>(new List<IRunnableCommand>() { this });
        }

        public ProcessResult Run(IRuntimeEngine engine)
        {
            var pipettor = engine.Instrument.Pipettor;
            var liquidManager = engine.Instrument.LiquidManager;

            var transferTargets = GetTransferContext(liquidManager);
            if (transferTargets.IsFailure)
            {
                //TODO Insufficient liquid error
            }

            var processResult = pipettor.Aspirate(new AspirateContext(transferTargets.Value, _parameters));
            if (!processResult.ContainsErrors)
            {
                foreach (var target in transferTargets.Value.GetActiveChannels())
                {
                    liquidManager.RemoveLiquidFromPosition(target.Container.Address, target.Volume);
                }
            }

            //return processResult;

            return new ProcessResult();
        }

        private Result<ChannelPattern<ChannelPipettingTransfer>> GetTransferContext(ILiquidManager liquidManager)
        {
            var volumeUsagePerLiquid = new Dictionary<string, double>();
            foreach (var liquidTarget in TransferGroup.GetActiveChannels())
            {
                //TODO
                //volumeUsagePerLiquid[liquidTarget.Source.GetId()] += liquidTarget.Target.Volume;
            }

            var transferContext = new ChannelPattern<ChannelPipettingTransfer>(TransferGroup.NumChannels);
            for(int i = 0; i < TransferGroup.NumChannels; ++i)
            {
                if(!TransferGroup.IsInUse(i))
                {
                    continue;
                }

                var transferTarget = liquidManager.RequestLiquid(TransferGroup[i]!.Source, volumeUsagePerLiquid[TransferGroup[i]!.Source.GetId()]);
                //If this happens then there's not enough liquid
                if (transferTarget.IsFailure)
                {
                    return Result.Failure<ChannelPattern<ChannelPipettingTransfer>>(transferTarget.Error);
                }

                double volume = TransferGroup[i]!.GetTotalTransferVolume() + AdditionalAspirateVolume;
                transferContext[i] = new ChannelPipettingTransfer(volume, TransferGroup[i]!.Source,
                    _parameters,
                    i,
                    transferTarget.Value,
                    TransferType.Aspirate);
            }
           
            return Result.Ok(transferContext);
        }

        public ResourcesUsage CalculateResources(IRuntimeEngine engine)
        {
            return ResourcesUsed;
        }
    }
}
