using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Runtime.Compilation
{
    public class DefaultCompiler : ICompiler
    {
        private IResourceInitializer _resourceInitializer;
        public DefaultCompiler(IResourceInitializer? resourceInitializer = null)
        {
            if (resourceInitializer == null)
            {
                _resourceInitializer = new DefaultResourceAutoInitializer();
            }
            else
            {
                _resourceInitializer = resourceInitializer;
            }
        }
        public Result Compile(IRunnable runnable, IRuntimeEngine engine)
        {
            var snapshot = engine.GetSnapshot();

            var commands = runnable.GetCommands(snapshot);
            if(commands.IsFailure)
            {
                Result.Failure($"Compile error: Unable to build commands for runnable");
            }

            var resourcesUsed = new ResourcesUsage();
            resourcesUsed.Combine(commands.Value.Select(x => x.ResourcesUsed).ToArray());

            var initialized = _resourceInitializer.Initialize(snapshot.Instrument, resourcesUsed);
            if(initialized.IsFailure)
            {
                return initialized;
            }



            return Result.Success();

        }


    }
}
