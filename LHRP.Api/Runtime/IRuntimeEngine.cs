using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime.Compilation;
using LHRP.Api.Runtime.ErrorHandling;

namespace LHRP.Api.Runtime
{
    public interface IRuntimeEngine : IStateSnapshotGetter<IRuntimeEngine>
    {
        IInstrument Instrument { get; }
        IRuntimeCommandQueue Commands { get; }
        IErrorHandler ErrorHandler { get; }
        RuntimeStatus Status { get; }
        ProcessResult Run();

        void Abort();

        ICompilationEngine GetCompilationEngine();
    }
}