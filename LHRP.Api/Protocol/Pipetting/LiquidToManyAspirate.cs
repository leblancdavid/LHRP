using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument.LiquidManagement;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.LiquidTransfers;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Scheduling;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Protocol.Pipetting
{
    public class LiquidToManyAspirate : IPipettingCommand
    {
        public Guid CommandId { get; private set; }
        private AspirateParameters _parameters;
        public TransferGroup<LiquidToManyTransfer> TransferGroup { get; private set; }
        public double AdditionalAspirateVolume { get; set; }
        public int RetryCount { get; private set; }

        public LiquidToManyAspirate(AspirateParameters parameters,
            TransferGroup<LiquidToManyTransfer> transferGroup,
            double additionalAspirateVolume,
            int retryAttempt = 0)
        {
            _parameters = parameters;
            TransferGroup = transferGroup;
            AdditionalAspirateVolume = additionalAspirateVolume;
            CommandId = Guid.NewGuid();
            RetryCount = retryAttempt;
        }


        public void ApplyChannelMask(ChannelPattern channelPattern)
        {
            TransferGroup.ChannelPattern = channelPattern;
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

            var processResult = pipettor.Aspirate(_parameters, transferTargets.Value, TransferGroup.ChannelPattern);
            if (!processResult.ContainsErrors)
            {
                foreach (var target in transferTargets.Value)
                {
                    liquidManager.RemoveLiquidFromPosition(target.Address, target.Volume);
                }
            }

            return processResult;
        }

        public Schedule Schedule(IRuntimeEngine runtimeEngine, bool initializeResources)
        {
            var schedule = new Schedule();
            foreach (var target in TransferGroup.Transfers)
            {
                schedule.ResourcesUsage.AddConsumableLiquidUsage(target.Source, target.GetTotalTransferVolume());
            }

            //Todo: come up with a way to calculate time
            schedule.ExpectedDuration = new TimeSpan(0, 0, 5);

            return schedule;
        }

        private Result<List<TransferTarget>> GetTransferTargets(ILiquidManager liquidManager)
        {
            var volumeUsagePerLiquid = new Dictionary<string, double>();
            foreach (var liquidTarget in TransferGroup.Transfers)
            {
                volumeUsagePerLiquid[liquidTarget.Source.AssignedId] += liquidTarget.GetTotalTransferVolume() + AdditionalAspirateVolume;
            }

            var transferTargets = new List<TransferTarget>();
            foreach (var liquidTarget in TransferGroup.Transfers)
            {
                //First we need to make sure there's enough liquid in the container to complete the transfer
                var transferTarget = liquidManager.RequestLiquid(liquidTarget.Source, volumeUsagePerLiquid[liquidTarget.Source.AssignedId]);
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
