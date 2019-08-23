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
        }

        public Process Run(IRuntimeEngine engine)
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

            return schedule;
        }
    }
}