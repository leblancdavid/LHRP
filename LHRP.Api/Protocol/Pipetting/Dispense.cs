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
        private DispenseParameters _parameters;
        private List<TransferTarget> _targets;
        public IEnumerable<TransferTarget> Targets => _targets;
        public ChannelPattern Pattern { get; set; }
        public ResourcesUsage ResourcesUsed { get; private set; }
        public Dispense(DispenseParameters parameters,
            List<TransferTarget> targets,
            ChannelPattern pattern,
            int retryAttempt = 0)
        {
            _parameters = parameters;
            _targets = targets;
            Pattern = pattern;
            CommandId = Guid.NewGuid();
            RetryCount = retryAttempt;

            ResourcesUsed = new ResourcesUsage();
            foreach (var target in _targets)
            {
                ResourcesUsed.AddTransfer(target);
            }
        }

        public void ApplyChannelMask(ChannelPattern channelPattern)
        {
            Pattern = channelPattern;
        }

        public Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            return Result.Ok<IEnumerable<IRunnableCommand>>(new List<IRunnableCommand>() { this });
        }

        public ProcessResult Run(IRuntimeEngine engine)
        {
            var pipettor = engine.Instrument.Pipettor;
            var liquidManager = engine.Instrument.LiquidManager;

            var processResult = pipettor.Dispense(_parameters, _targets, Pattern);
            if(!processResult.ContainsErrors)
            {
                //foreach (var target in _targets)
                //{
                //    liquidManager.AddLiquidToPosition(target.Address, target.Liquid, target.Volume);
                //}
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