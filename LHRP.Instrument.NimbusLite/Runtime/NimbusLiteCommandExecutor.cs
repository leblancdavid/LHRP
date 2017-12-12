using System;
using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Runtime
{
    public class NimbusLiteCommandExecutor : ICommandExecutor
    {
        public NimbusLiteCommandExecutor()
        {

        }

        public void ExecuteCommand(Command command)
        {
            //Todo send the command down to the devices
            Console.WriteLine(command.ToString());
        }
    }
}