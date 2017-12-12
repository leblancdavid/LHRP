using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Runtime
{
    public class NimbusLiteCommandScheduler : ICommandScheduler
    {
        public void ExecuteCommand(Command command)
        {
            
        }

        public Schedule GetSchedule()
        {
            return new Schedule();
        }
    }
}