using LHRP.Api.Runtime;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime.Scheduling;
using System;

namespace LHRP.Api.Protocol.Pipetting
{
    public class Aspirate : IRunnableCommand
    {
        private AspirateParameters _parameters;
        public Aspirate(AspirateParameters parameters)
        {
            _parameters = parameters;
            CommandId = Guid.NewGuid();
        }

        public Guid CommandId { get; private set; }

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