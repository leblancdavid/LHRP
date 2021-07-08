using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.LiquidTransfers;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.Resources;
using LHRP.Api.Runtime.Scheduling;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Protocol.Pipetting
{
    public class LiquidToOneAspirate : IPipettingCommand
    {
        public Guid CommandId { get; private set; }
        private AspirateContext _parameters;
        public TransferGroup<LiquidToOneTransfer> TransferGroup { get; private set; }
        public int RetryCount { get; private set; }
        public ResourcesUsage ResourcesUsed { get; private set; }

        public LiquidToOneAspirate(AspirateContext parameters,
            TransferGroup<LiquidToOneTransfer> transferGroup,
            int retryAttempt = 0)
        {
            _parameters = parameters;
            TransferGroup = transferGroup;
            CommandId = Guid.NewGuid();
            RetryCount = retryAttempt;

            ResourcesUsed = new ResourcesUsage();
            foreach (var target in TransferGroup.Transfers)
            {
                ResourcesUsed.AddConsumableLiquidUsage(target.Source, target.Target.Volume);
            }
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

            RuntimeError? error;
            var transferTargets = GetTransferTargets(engine, liquidManager, out error);
            if(transferTargets.IsFailure)
            {
                return new ProcessResult(error!);
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

        private Result<List<TransferTarget>> GetTransferTargets(IRuntimeEngine engine, ILiquidManager liquidManager, out RuntimeError? error)
        {
            var volumeUsagePerLiquid = new Dictionary<string, double>();
            foreach(var liquidTarget in TransferGroup.Transfers)
            {
                if(!volumeUsagePerLiquid.ContainsKey(liquidTarget.Source.GetId()))
                {
                    volumeUsagePerLiquid[liquidTarget.Source.GetId()] = 0.0;
                }

                volumeUsagePerLiquid[liquidTarget.Source.GetId()] += liquidTarget.Target.Volume;
            }
            
            var transferTargets = new List<TransferTarget>();
            foreach (var liquidTarget in TransferGroup.Transfers)
            {
                //First we need to make sure there's enough liquid in the container to complete the transfer
                var transferTarget = liquidManager.RequestLiquid(liquidTarget.Source, volumeUsagePerLiquid[liquidTarget.Source.GetId()]);
                //If this happens then there's not enough liquid
                if(transferTarget.IsFailure)
                {
                    error = new InsufficientLiquidRuntimeError(transferTarget.Error, liquidTarget.Source,
                        GetRemainingRequiredLiquidVolume(engine, liquidTarget.Source));
                    return Result.Failure<List<TransferTarget>>(transferTarget.Error);
                }

                transferTarget.Value.Volume = liquidTarget.Target.Volume;
                transferTargets.Add(transferTarget.Value);
            }

            error = null;
            return Result.Ok(transferTargets);
        }
        private double GetRemainingRequiredLiquidVolume(IRuntimeEngine engine, Liquid liquid)
        {
            var resources = engine.Commands.GetRemainingResources();
            return resources.ConsumableLiquidUsages[liquid];
        }
    }

    
}
