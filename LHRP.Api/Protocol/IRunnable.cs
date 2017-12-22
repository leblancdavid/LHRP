using LHRP.Api.Common;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol
{
    public interface IRunnable
    {
         Result<Process> Run(IInstrument instrument);
    }
}