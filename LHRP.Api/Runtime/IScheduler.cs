using LHRP.Api.Protocol;

namespace LHRP.Api.Runtime
{
    public interface IScheduler
    {
         Schedule Schedule(IRunnable run);
    }
}