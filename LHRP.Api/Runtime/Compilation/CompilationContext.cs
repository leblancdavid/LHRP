using LHRP.Api.Instrument;
using LHRP.Api.Runtime.Resources;

namespace LHRP.Api.Runtime.Compilation
{
    public class CompilationContext
    {
        public IInstrument InstrumentSnapshot { get; private set; }
        public ResourcesUsage ResourcesUsed { get; private set; }
        public CompilationContext(IInstrument instrument, ResourcesUsage resourcesUsage)
        {
            InstrumentSnapshot = instrument;
            ResourcesUsed = resourcesUsage;
        }
    }
}
