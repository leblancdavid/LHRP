using LHRP.Api.Instrument;
using LHRP.Api.Protocol;

namespace LHRP.Api.Runtime
{
    public interface IRuntimeEngine
    {
        IInstrument Instrument { get; }
        ProcessResult Run(IRunnable run);
    }
}