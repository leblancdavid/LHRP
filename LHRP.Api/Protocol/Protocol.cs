using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Compilation;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.Resources;
using LHRP.Api.Runtime.Scheduling;

namespace LHRP.Api.Protocol
{
    public class Protocol : IRunnable
    {
        private List<IRunnable> _steps = new List<IRunnable>();
        public IEnumerable<IRunnable> Steps => _steps;


        public Protocol()
        {
        }

        public void AddStep(IRunnable step)
        {
            _steps.Add(step);
        }

        public Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            var runnableCommands = new List<IRunnableCommand>();
            foreach(var step in _steps)
            {
                var stepCommands = step.GetCommands(engine);
                if(stepCommands.IsFailure)
                {
                    return stepCommands;
                }
                runnableCommands.AddRange(stepCommands.Value);
            }

            return Result.Ok<IEnumerable<IRunnableCommand>>(runnableCommands);
        }

        public ProcessResult Run(IRuntimeEngine runtime)
        {
            var process = new ProcessResult();
            var commands = GetCommands(runtime);

            if(commands.IsFailure)
            {
                process.AddError(new RuntimeError(commands.Error));
                return process;
            }

            foreach(var command in commands.Value)
            {
                runtime.Commands.Add(command);
            }

            return runtime.Run();
        }

        public Result<Schedule> Schedule(IRuntimeEngine runtimeEngine, bool initializeResources)
        {
            var schedule = new Schedule();
            foreach(var step in _steps)
            {
                var stepSchedule = step.Schedule(runtimeEngine, false);
                schedule.Combine(stepSchedule.Value);
            }

            if (initializeResources)
            {
                return runtimeEngine.Instrument.InitializeResources(schedule);
            }
            return Result.Success(schedule);
        }

        public ProcessResult Compile(ICompilationEngine engine)
        {
            var process = new ProcessResult();
            foreach (var step in _steps)
            {
                step.Compile(engine);
            }

            return process;
        }
    }
}