namespace LHRP.Api.Runtime
{
    public interface ISchedulingEngine : IRuntimeEngine
    {
         Schedule GetSchedule();
    }
}