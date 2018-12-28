using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime;
using LHRP.Api.Devices.Pipettor;

namespace LHRP.Api.Protocol.Pipetting
{
    public class Aspirate : IRunnable
    {
        private AspirateParameters _parameters;
        public Aspirate(AspirateParameters parameters)
        {
            _parameters = parameters;
        }

        public Process Run(IRuntimeEngine engine)
        {
            return engine.Instrument.Pipettor.Aspirate(_parameters);
        }
    }
}