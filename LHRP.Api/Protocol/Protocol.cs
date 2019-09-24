using System.Collections.Generic;
using CSharpFunctionalExtensions;
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

        public Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            throw new System.NotImplementedException();
        }

        public ProcessResult Run(IRuntimeEngine runtime)
        {
            var process = new ProcessResult();
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
            var schedule = new Schedule();
            foreach(var step in _steps)
            {
                var stepSchedule = step.Schedule(runtimeEngine);
                schedule.Combine(stepSchedule);
            }

            return schedule;
        }
    }
}