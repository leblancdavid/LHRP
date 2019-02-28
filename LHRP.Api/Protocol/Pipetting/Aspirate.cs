using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime;
using LHRP.Api.Devices.Pipettor;

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
    }
}