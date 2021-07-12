using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.Resources;
using LHRP.Api.Runtime.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Protocol.Pipetting
{
    public class Dispense : IPipettingCommand
    {
        public Guid CommandId { get; private set; }
        public int RetryCount { get; private set; }
        private DispenseParameters _parameters;
        private ChannelPattern<TransferTarget> _targets;
        public ResourcesUsage ResourcesUsed { get; private set; }
        public Dispense(DispenseParameters parameters,
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

        public void ApplyChannelMask(ChannelPattern channelPattern)
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

            var errors = new List<RuntimeError>();
            var pipetteTargets = _targets.ToChannelPatternPipettingContext(engine.Instrument, out errors);
            if (errors.Any())
            {
                return new ProcessResult(errors.ToArray());
            }

            var processResult = pipettor.Dispense(new DispenseContext(pipetteTargets, _parameters));
            if(!processResult.ContainsErrors)
            {
                for(int i = 0; i < _targets.NumChannels; ++i)
                {
                    if(pipettorStatus[i].HasTip && pipettorStatus[i].ContainsLiquid && _targets.IsInUse(i))
                    {
                        liquidManager.AddLiquidToPosition(_targets[i]!.Address, pipettorStatus[i].CurrentLiquid!, _targets[i]!.Volume);
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

        public ProcessResult Compile(IRuntimeEngine engine)
        {
            var processResult = new ProcessResult();

            var pipettor = engine.Instrument.Pipettor;
            var liquidManager = engine.Instrument.LiquidManager;
            var pipettorStatus = pipettor.PipettorStatus;


            var errors = new List<RuntimeError>();
            var pipetteTargets = _targets.ToChannelPatternPipettingContext(engine.Instrument, out errors);

            return processResult;
        }
    }
}