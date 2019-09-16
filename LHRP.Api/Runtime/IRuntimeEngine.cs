using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime.ErrorHandling;

namespace LHRP.Api.Runtime
{
    public interface IRuntimeEngine
    {
        IInstrument Instrument { get; }
        IRuntimeCommandQueue Commands { get; }
        IErrorHandler ErrorHandler { get; }

        ProcessResult Run();
    }
}