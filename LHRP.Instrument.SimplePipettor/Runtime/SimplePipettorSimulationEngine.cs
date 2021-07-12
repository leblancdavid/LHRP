using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Instrument.SimplePipettor.Instrument;
using LHRP.Instrument.SimplePipettor.Runtime.ErrorHandling;

namespace LHRP.Instrument.SimplePipettor.Runtime
{
    public class SimplePipettorSimulationEngine : BaseRuntimeEngine, IRuntimeEngine
    {
        public SimplePipettorSimulationEngine() 
            :base(new SimpleSimulatedPipettorInstrument(), 
                 new RuntimeCommandQueue(), 
                 new SimplePipettorErrorHandler())
        {
        }
    }
}