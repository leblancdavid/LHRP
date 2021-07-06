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
    public class LiquidToOneAspirate : IPipettingCommand
    {
        public Guid CommandId { get; private set; }
        private AspirateParameters _parameters;
        public TransferGroup<LiquidToOneTransfer> TransferGroup { get; private set; }
        public int RetryCount { get; private set; }

        public LiquidToOneAspirate(AspirateParameters parameters,
            TransferGroup<LiquidToOneTransfer> transferGroup,
            int retryAttempt = 0)
        {
            _parameters = parameters;
            TransferGroup = transferGroup;
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
            if(transferTargets.IsFailure)
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
                schedule.ResourcesUsage.AddConsumableLiquidUsage(target.Source, target.Target.Volume);
            }

            //Todo: come up with a way to calculate time
            schedule.ExpectedDuration = new TimeSpan(0, 0, 5);

            return schedule;
        }

        private Result<List<TransferTarget>> GetTransferTargets(ILiquidManager liquidManager)
        {
            var volumeUsagePerLiquid = new Dictionary<string, double>();
            foreach(var liquidTarget in TransferGroup.Transfers)
            {
                if(!volumeUsagePerLiquid.ContainsKey(liquidTarget.Source.AssignedId))
                {
                    volumeUsagePerLiquid[liquidTarget.Source.AssignedId] = 0.0;
                }

                volumeUsagePerLiquid[liquidTarget.Source.AssignedId] += liquidTarget.Target.Volume;
            }
            
            var transferTargets = new List<TransferTarget>();
            foreach (var liquidTarget in TransferGroup.Transfers)
            {
                //First we need to make sure there's enough liquid in the container to complete the transfer
                var transferTarget = liquidManager.RequestLiquid(liquidTarget.Source, volumeUsagePerLiquid[liquidTarget.Source.AssignedId]);
                //If this happens then there's not enough liquid
                if(transferTarget.IsFailure)
                {
                    return Result.Failure<List<TransferTarget>>(transferTarget.Error);
                }

                transferTarget.Value.Volume = liquidTarget.Target.Volume;
                transferTargets.Add(transferTarget.Value);
            }
            
            return Result.Ok(transferTargets);
        }
    }
}
