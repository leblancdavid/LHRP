using System;
using System.Collections.Generic;

namespace LHRP.Api.Runtime
{
    public class Process
    {
        private List<Process> _subProcess = new List<Process>();
        public IEnumerable<Process> SubProcess => _subProcess;
        public TimeSpan Duration { get; private set; }
        public TimeSpan EstimatedDuration { get; set; }

        public Process()
        {
            Duration = new TimeSpan(0);
            EstimatedDuration = new TimeSpan(0);
        }

        public Process(TimeSpan estimate, TimeSpan duration)
        {
            Duration = duration;
            EstimatedDuration = estimate;
        }

        public void AppendSubProcess(Process subProcess)
        {
            Duration += subProcess.Duration;
            EstimatedDuration += subProcess.EstimatedDuration;
            _subProcess.Add(subProcess);
        }
    }
}