using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Errors;
using LHRP.Instrument.SimplePipettor.Instrument;

namespace LHRP.Instrument.SimplePipettor.Runtime
{
    public class SimplePipettorRuntimeEngine : BaseRuntimeEngine, IRuntimeEngine
    {
        public SimplePipettorRuntimeEngine() 
            :base(new SimplePipettorInstrument(), new RuntimeCommandQueue(), new DefaultErrorHandler())
        {
        }
    }
}