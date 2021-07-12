using CSharpFunctionalExtensions;
using LHRP.Api.Runtime.Compilation;
using LHRP.Api.Runtime.Scheduling;
using System.Collections.Generic;

namespace LHRP.Api.Runtime
{
    public interface IRunnable : ISchedulable
    {
        ProcessResult Run(IRuntimeEngine engine);
    }
}