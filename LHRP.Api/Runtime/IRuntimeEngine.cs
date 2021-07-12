using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime.Compilation;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.Resources;

namespace LHRP.Api.Runtime
{
    public interface IRuntimeEngine : IStateSnapshotGetter<IRuntimeEngine>, ISimulatable<IRuntimeEngine>
    {
        IInstrument Instrument { get; }
        IRuntimeCommandQueue Commands { get; }
        IErrorHandler ErrorHandler { get; }
        RuntimeStatus Status { get; }
        ProcessResult Run(IResourceInitializer? resourceInitializer = null);

        void Abort();

        ICompilationEngine GetCompilationEngine();
    }
}