using LHRP.Api.Runtime.Scheduling;

namespace LHRP.Api.Runtime
{
    public interface IRunnable : ISchedulable
    {
         ProcessResult Run(IRuntimeEngine enigne);
    }
}