using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol.Pipetting
{
    public class Aspirate : IRunnable
    {
        private AspirateOptions _options;
        public Aspirate(AspirateOptions options)
        {
            _options = options;
        }

        public Result<Process> Run(IInstrument instrument)
        {
            throw new System.NotImplementedException();
        }
    }
}