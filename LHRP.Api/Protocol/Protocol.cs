using System.Collections.Generic;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol
{
    public class Protocol : IRunnable
    {
        private List<IRunnable> _steps;
        public IEnumerable<IRunnable> Steps => _steps;
        public void AddStep(IRunnable step)
        {
            _steps.Add(step);
        }

        public void Run(ICommandExecutor commandExecutor)
        {
            foreach(var step in _steps)
            {
                step.Run(commandExecutor);
            }
        }
    }
}