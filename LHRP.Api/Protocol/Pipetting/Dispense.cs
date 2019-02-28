using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol.Pipetting
{
    public class Dispense : IRunnableCommand
    {
        private DispenseParameters _parameters;
        public Dispense(DispenseParameters parameters)
        {
            _parameters = parameters;
        }

        public Process Run(IRuntimeEngine engine)
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
    }
}