using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Scheduling;
using System;

namespace LHRP.Api.Protocol.Pipetting
{
    public class Dispense : IRunnableCommand
    {
        private DispenseParameters _parameters;
        public Dispense(DispenseParameters parameters)
        {
            _parameters = parameters;
        }

        public ProcessResult Run(IRuntimeEngine engine)
        {
            var pipettor = engine.Instrument.Pipettor;
            var liquidManager = engine.Instrument.LiquidManager;

            var processResult = pipettor.Dispense(_parameters);
            if(!processResult.ContainsErrors)
            {
                foreach(var target in _parameters.Targets)
                {
                    liquidManager.AddLiquidToPosition(target.Address, target.Liquid, target.Volume);
                }
            }
            
            return processResult;
        }

        public Schedule Schedule(IRuntimeEngine runtimeEngine)
        {
            var schedule = new Schedule();
            foreach (var target in _parameters.Targets)
            {
                schedule.ResourcesUsage.AddTransfer(target);
            }

            //Todo: come up with a way to calculate time
            schedule.ExpectedDuration = new TimeSpan(0, 0, 5);

            return schedule;
        }
    }
}