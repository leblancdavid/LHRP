namespace LHRP.Api.Devices
{
    public class CommandProcessor : ICommandProcessor
    {
        public CommandProcessor()
        {

        }

        CommandResult ICommandProcessor.Process(Command command)
        {
            throw new System.NotImplementedException();
        }
    }
}