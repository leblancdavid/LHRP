using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol.Pipetting
{
    public class Dispense : IRunnable
    {
        private DispenseParameters _parameters;
        public Dispense(DispenseParameters parameters)
        {
            _parameters = parameters;
        }

        public Process Run(IInstrument instrument)
        {
            return instrument.Pipettor.Dispense(_parameters);
        }
    }
}