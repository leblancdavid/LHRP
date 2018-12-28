using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Errors;
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

        public IRuntimeCommandQueue Commands { get; private set; }

        public IErrorHandler ErrorHandler => throw new System.NotImplementedException();

        public Process Run()
        {
            throw new System.NotImplementedException();
        }
    }
}