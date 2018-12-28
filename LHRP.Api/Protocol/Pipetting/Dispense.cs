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
            return engine.Instrument.Pipettor.Dispense(_parameters);
        }
    }
}