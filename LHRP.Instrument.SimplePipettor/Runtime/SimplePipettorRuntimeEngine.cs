using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;
using LHRP.Instrument.SimplePipettor.Instrument;

namespace LHRP.Instrument.SimplePipettor.Runtime
{
    public class SimplePipettorRuntimeEngine : IRuntimeEngine
    {
        public SimplePipettorRuntimeEngine()
        {
            Instrument = new SimplePipettorInstrument();
        }
        public IInstrument Instrument { get; private set; }

        public Process Run(IRunnable run)
        {
            return run.Run(this);
        }
    }
}