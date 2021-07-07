using CSharpFunctionalExtensions;
using LHRP.Api.Runtime.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Runtime.Scheduling
{
    public interface ISchedulable
    {
        Result<Schedule> Schedule(IRuntimeEngine runtimeEngine, bool initializeResources);
        Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine);
    }
}
