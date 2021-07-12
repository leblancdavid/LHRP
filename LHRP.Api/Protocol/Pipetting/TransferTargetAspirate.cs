using LHRP.Api.Runtime;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime.Scheduling;
using System;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime.Resources;
using LHRP.Api.Runtime.ErrorHandling;
using System.Linq;

namespace LHRP.Api.Protocol.Pipetting
{
    public class TransferTargetAspirate : IPipettingCommand
    {
        public Guid CommandId { get; private set; }
        private AspirateParameters _parameters;
        private ChannelPattern<TransferTarget> _targets;
        public int RetryCount { get; private set; }

        public ResourcesUsage ResourcesUsed { get; private set; }

        public TransferTargetAspirate(AspirateParameters parameters,
            ChannelPattern<TransferTarget> targets,
            int retryAttempt = 0)
        {
            _parameters = parameters;
            _targets = targets;
            CommandId = Guid.NewGuid();
            RetryCount = retryAttempt;

            ResourcesUsed = new ResourcesUsage();
            foreach (var target in _targets.GetActiveChannels())
            {
                ResourcesUsed.AddTransfer(target);
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

            var errors = new List<RuntimeError>();
            var pipetteTargets = _targets.ToChannelPatternPipettingContext(engine.Instrument, out errors);
            if(errors.Any())
            {
                return new ProcessResult(errors.ToArray());
            }
            var processResult = pipettor.Aspirate(new AspirateContext(pipetteTargets, _parameters));
            if(!processResult.ContainsErrors)
            {
                foreach (var target in _targets.GetActiveChannels())
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

        public ProcessResult Compile(IRuntimeEngine engine)
        {
            throw new NotImplementedException();
        }
    }
}