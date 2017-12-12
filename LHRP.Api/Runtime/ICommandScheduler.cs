namespace LHRP.Api.Runtime
{
    public interface ICommandScheduler : ICommandExecutor
    {
         Schedule GetSchedule();
    }
}