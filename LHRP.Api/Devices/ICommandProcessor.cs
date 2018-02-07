namespace LHRP.Api.Devices
{
    public interface ICommandProcessor
    {
        CommandResult Process(Command command);
    }
}