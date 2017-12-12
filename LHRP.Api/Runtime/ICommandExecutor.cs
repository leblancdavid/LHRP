namespace LHRP.Api.Runtime
{
    public interface ICommandExecutor
    {
         void ExecuteCommand(Command command);
    }
}