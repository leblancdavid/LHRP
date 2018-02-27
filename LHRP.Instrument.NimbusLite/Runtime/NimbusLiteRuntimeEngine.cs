using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;
using LHRP.Instrument.NimbusLite.Instrument;

namespace LHRP.Instrument.NimbusLite.Runtime
{
    public class NimbusLiteRuntimeEngine : IRuntimeEngine
    {
        public NimbusLiteRuntimeEngine()
        {
            Instrument = new NimbusLiteInstrument();
        }
        public IInstrument Instrument { get; private set; }

        public Result<Process> Run(IRunnable run)
        {
            return run.Run(Instrument);
        }
    }
}