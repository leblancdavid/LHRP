using System;
using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Runtime
{
    public class NimbusLiteCommandSimulator : ICommandExecutor
    {
        public void ExecuteCommand(Command command)
        {
            Console.WriteLine(command.ToString());
        }
    }
}