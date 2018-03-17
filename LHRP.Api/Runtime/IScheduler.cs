using CSharpFunctionalExtensions;
using LHRP.Api.Protocol;

namespace LHRP.Api.Runtime
{
    public interface IScheduler
    {
         Process Schedule(IRunnable run);
    }
}