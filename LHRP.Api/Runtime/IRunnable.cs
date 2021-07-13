using CSharpFunctionalExtensions;
using LHRP.Api.Runtime.Resources;
using System.Collections.Generic;

namespace LHRP.Api.Runtime
{
    public interface IRunnable
    {
        ProcessResult Run(IRuntimeEngine engine);
        ResourcesUsage CalculateResources(IRuntimeEngine engine);
        Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine);
    }
}