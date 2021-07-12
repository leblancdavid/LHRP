using CSharpFunctionalExtensions;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Protocol.Steps
{
    public class CustomStep : IRunnable
    {
        protected List<IRunnableCommand> _commands;
        public IEnumerable<IRunnableCommand> Commands => _commands;

        public CustomStep()
        {
            _commands = new List<IRunnableCommand>();
        }

        public CustomStep(IEnumerable<IRunnableCommand> commands)
        {
            _commands = commands.ToList();
        }

        public void AddCommand(IRunnableCommand command)
        {
            _commands.Add(command);
        }

        public Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            return Result.Success(Commands);
        }

        public ProcessResult Run(IRuntimeEngine engine)
        {
            var process = new ProcessResult();

            foreach (var command in _commands)
            {
                engine.Commands.Add(command);
            }

            return engine.Run();
        }

        public Result<Schedule> Schedule(IRuntimeEngine runtimeEngine, bool initializeResources)
        {
            throw new NotImplementedException();
        }
    }
}
