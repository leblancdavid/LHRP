using CSharpFunctionalExtensions;
using LHRP.Api.Runtime.Resources;
using LHRP.Api.Runtime.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Runtime
{
    public abstract class BaseRunnable : IRunnable
    {
        public virtual ResourcesUsage CalculateResources(IRuntimeEngine engine)
        {
            var resources = new ResourcesUsage();
            var commands = GetCommands(engine);
            if (commands.IsFailure)
            {
                return resources;
            }

            resources.Combine(commands.Value.Select(x => x.ResourcesUsed).ToArray());
            return resources;
        }

        public abstract Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine);

        public abstract ProcessResult Run(IRuntimeEngine engine);

        public abstract Result<Schedule> Schedule(IRuntimeEngine runtimeEngine, bool initializeResources);
    }
}
