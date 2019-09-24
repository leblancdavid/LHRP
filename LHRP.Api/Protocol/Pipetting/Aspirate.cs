using LHRP.Api.Runtime;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime.Scheduling;
using System;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Protocol.Pipetting
{
    public class Aspirate : IPipettingCommand
    {
        public Guid CommandId { get; private set; }
        private AspirateParameters _parameters;
        private List<TransferTarget> _targets;
        public IEnumerable<TransferTarget> Targets => _targets;
        public ChannelPattern Pattern { get; set; }
        public int RetryCount { get; private set; }

        public Aspirate(AspirateParameters parameters, 
            List<TransferTarget> targets, 
            ChannelPattern pattern,
            int retryAttempt = 0)
        {
            _parameters = parameters;
            _targets = targets;
            Pattern = pattern;
            CommandId = Guid.NewGuid();
            RetryCount = retryAttempt;
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

            var processResult = pipettor.Aspirate(_parameters, _targets, Pattern);
            if(!processResult.ContainsErrors)
            {
                foreach(var target in _targets)
                {
                    liquidManager.RemoveLiquidFromPosition(target.Address, target.Volume);
                }
            }
            
            return processResult;
        }

        public Schedule Schedule(IRuntimeEngine runtimeEngine)
        {
            var schedule = new Schedule();
            foreach(var target in _targets)
            {
                schedule.ResourcesUsage.AddTransfer(target);
            }

            //Todo: come up with a way to calculate time
            schedule.ExpectedDuration = new TimeSpan(0, 0, 5);

            return schedule;
        }
    }
}