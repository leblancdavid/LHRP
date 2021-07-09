using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.LiquidTransfers;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Resources;
using LHRP.Api.Runtime.Scheduling;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Protocol.Pipetting
{
    public class LiquidToManyAspirate : IPipettingCommand
    {
        public Guid CommandId { get; private set; }
        private AspirateContext _parameters;
        public ChannelPattern<LiquidToManyTransfer> TransferGroup { get; private set; }
        public double AdditionalAspirateVolume { get; set; }
        public int RetryCount { get; private set; }
        public ResourcesUsage ResourcesUsed { get; private set; }

        public LiquidToManyAspirate(AspirateContext parameters,
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
            //foreach (var target in TransferGroup.Transfers)
            //{
            //    ResourcesUsed.AddConsumableLiquidUsage(target.Source, target.GetTotalTransferVolume());
            //}
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

            var transferTargets = GetTransferTargets(liquidManager);
            if (transferTargets.IsFailure)
            {
                //TODO Insufficient liquid error
            }

            //var processResult = pipettor.Aspirate(_parameters, TransferGroup);
            //if (!processResult.ContainsErrors)
            //{
            //    foreach (var target in transferTargets.Value)
            //    {
            //        liquidManager.RemoveLiquidFromPosition(target.Address, target.Volume);
            //    }
            //}

            //return processResult;

            return new ProcessResult();
        }

        public Result<Schedule> Schedule(IRuntimeEngine runtimeEngine, bool initializeResources)
        {
            var schedule = new Schedule();
            schedule.ResourcesUsage.Combine(ResourcesUsed);

            //Todo: come up with a way to calculate time
            schedule.ExpectedDuration = new TimeSpan(0, 0, 5);
            if (initializeResources)
            {
                return runtimeEngine.Instrument.InitializeResources(schedule);
            }
            return Result.Success(schedule);
        }

        private Result<List<TransferTarget>> GetTransferTargets(ILiquidManager liquidManager)
        {
            var volumeUsagePerLiquid = new Dictionary<string, double>();
            foreach (var liquidTarget in TransferGroup.GetActiveChannels())
            {
                volumeUsagePerLiquid[liquidTarget.Source.GetId()] += liquidTarget.GetTotalTransferVolume() + AdditionalAspirateVolume;
            }

            var transferTargets = new List<TransferTarget>();
            foreach (var liquidTarget in TransferGroup.GetActiveChannels())
            {
                //First we need to make sure there's enough liquid in the container to complete the transfer
                var transferTarget = liquidManager.RequestLiquid(liquidTarget.Source, volumeUsagePerLiquid[liquidTarget.Source.GetId()]);
                //If this happens then there's not enough liquid
                if (transferTarget.IsFailure)
                {
                    return Result.Failure<List<TransferTarget>>(transferTarget.Error);
                }

                transferTarget.Value.Volume = liquidTarget.GetTotalTransferVolume() + AdditionalAspirateVolume;
                transferTargets.Add(transferTarget.Value);
            }

            return Result.Ok(transferTargets);
        }
    }
}
