using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Resources;
using LHRP.Api.Runtime.Scheduling;
using System;
using System.Collections.Generic;

namespace LHRP.Api.Protocol.Pipetting
{
    public class Dispense : IPipettingCommand
    {
        public Guid CommandId { get; private set; }
        public int RetryCount { get; private set; }
        private DispenseContext _parameters;
        private ChannelPattern<TransferTarget> _targets;
        public ResourcesUsage ResourcesUsed { get; private set; }
        public Dispense(DispenseContext parameters,
            ChannelPattern<TransferTarget> targets,
            int retryAttempt = 0)
        {
            _parameters = parameters;
            _targets = targets;
            CommandId = Guid.NewGuid();
            RetryCount = retryAttempt;

            ResourcesUsed = new ResourcesUsage();
            for(int i = 0; i < _targets.NumChannels; ++i)
            {
                if(_targets[i] != null)
                {
                    ResourcesUsed.AddTransfer(_targets[i]!);
                }
            }

        }

        public void ApplyChannelMask(ChannelPattern<bool> channelPattern)
        {
            _targets.Mask(channelPattern);
        }

        public Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            return Result.Ok<IEnumerable<IRunnableCommand>>(new List<IRunnableCommand>() { this });
        }

        public ProcessResult Run(IRuntimeEngine engine)
        {
            var pipettor = engine.Instrument.Pipettor;
            var liquidManager = engine.Instrument.LiquidManager;
            var pipettorStatus = pipettor.PipettorStatus;

            var processResult = pipettor.Dispense(_parameters, _targets, Pattern);
            if(!processResult.ContainsErrors)
            {
                for(int i = 0; i < _targets.Count; ++i)
                {
                    if(pipettorStatus[i].HasTip && pipettorStatus[i].ContainsLiquid)
                    {
                        liquidManager.AddLiquidToPosition(_targets[i].Address, pipettorStatus[i].CurrentLiquid!, _targets[i].Volume);
                    }
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

            if(initializeResources)
            {
                return runtimeEngine.Instrument.InitializeResources(schedule);
                
            }

            return Result.Success(schedule);
        }
    }
}