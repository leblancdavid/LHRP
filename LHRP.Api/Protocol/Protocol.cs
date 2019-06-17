using System.Collections.Generic;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Scheduling;

namespace LHRP.Api.Protocol
{
    public class Protocol : IRunnable
    {
        private List<IRunnable> _steps = new List<IRunnable>();
        public IEnumerable<IRunnable> Steps => _steps;
        public void AddStep(IRunnable step)
        {
            _steps.Add(step);
        }

        public Process Run(IRuntimeEngine runtime)
        {
            var process = new Process();
            foreach(var step in _steps)
            {
                var stepProcess = step.Run(runtime);
                if(stepProcess.ContainsErrors)
                {
                    //TODO handle errors
                }
                process.AppendSubProcess(stepProcess);
            }

            return process;
        }

        public Schedule Schedule(IRuntimeEngine runtimeEngine)
        {
            throw new System.NotImplementedException();
        }
    }
}