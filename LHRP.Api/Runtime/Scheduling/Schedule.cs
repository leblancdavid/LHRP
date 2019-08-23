using LHRP.Api.Runtime.Resources;

namespace LHRP.Api.Runtime.Scheduling
{
    public class Schedule
    {
        public ResourcesUsage ResourcesUsage { get; private set; }

        public Schedule()
        {
            ResourcesUsage = new ResourcesUsage();
        }

        public void Combine(params Schedule[] schedules)
        {
            for(int i = 0; i < schedules.Length; ++i)
            {
                ResourcesUsage.Combine(schedules[i].ResourcesUsage);
            }
        }
    }
}