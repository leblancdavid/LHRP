using CSharpFunctionalExtensions;
using LHRP.Api.Runtime.Compilation;
using LHRP.Api.Runtime.Resources;
using LHRP.Api.Runtime.Scheduling;
using System.Collections.Generic;

namespace LHRP.Api.Runtime
{
    public interface IRunnable : ISchedulable
    {
        ProcessResult Run(IRuntimeEngine engine);
        ResourcesUsage CalculateResources(IRuntimeEngine engine);
        Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine);
    }
}