using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace LHRP.Api.Runtime.Scheduling
{
    public interface ISchedulable
    {
        Result<Schedule> Schedule(IRuntimeEngine runtimeEngine, bool initializeResources);
    }
}
