using LHRP.Api.Runtime;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime.Scheduling;
using System;
using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace LHRP.Api.Protocol.Pipetting
{
    public class Aspirate : IPipettingCommand
    {
        private AspirateParameters _parameters;
        public Aspirate(AspirateParameters parameters, int retryAttempt = 0)
        {
            _parameters = parameters;
            CommandId = Guid.NewGuid();
            RetryCount = retryAttempt;
        }

        public Guid CommandId { get; private set; }

        public int RetryCount { get; private set; }

        public void ApplyChannelMask(ChannelPattern channelPattern)
        {
            _parameters.Pattern = channelPattern;
        }

        public Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            return Result.Ok<IEnumerable<IRunnableCommand>>(new List<IRunnableCommand>() { this });
        }

        public ProcessResult Run(IRuntimeEngine engine)
        {
            var pipettor = engine.Instrument.Pipettor;
            var liquidManager = engine.Instrument.LiquidManager;

            var processResult = pipettor.Aspirate(_parameters);
            if(!processResult.ContainsErrors)
            {
                foreach(var target in _parameters.Targets)
                {
                    liquidManager.RemoveLiquidFromPosition(target.Address, target.Volume);
                }
            }
            
            return processResult;
        }

        public Schedule Schedule(IRuntimeEngine runtimeEngine)
        {
            var schedule = new Schedule();
            foreach(var target in _parameters.Targets)
            {
                schedule.ResourcesUsage.AddTransfer(target);
            }

            //Todo: come up with a way to calculate time
            schedule.ExpectedDuration = new TimeSpan(0, 0, 5);

            return schedule;
        }
    }
}