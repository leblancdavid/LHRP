using LHRP.Api.Runtime.Scheduling;

namespace LHRP.Api.Runtime
{
    public interface IRunnable : ISchedulable
    {
         Process Run(IRuntimeEngine enigne);
    }
}