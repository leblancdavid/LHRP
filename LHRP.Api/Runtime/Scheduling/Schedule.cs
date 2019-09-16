using LHRP.Api.Runtime.Resources;
using System;

namespace LHRP.Api.Runtime.Scheduling
{
    public class Schedule
    {
        public ResourcesUsage ResourcesUsage { get; private set; }
        public TimeSpan ExpectedDuration { get; set; }

        public Schedule()
        {
            ResourcesUsage = new ResourcesUsage();
        }

        public void Combine(params Schedule[] schedules)
        {
            for(int i = 0; i < schedules.Length; ++i)
            {
                ResourcesUsage.Combine(schedules[i].ResourcesUsage);
                ExpectedDuration += schedules[i].ExpectedDuration;
            }
        }
    }
}