using LHRP.Api.Common;
using LHRP.Api.Protocol;

namespace LHRP.Api.Runtime
{
    public interface IScheduler
    {
         Result<Process> Schedule(IRunnable run);
    }
}