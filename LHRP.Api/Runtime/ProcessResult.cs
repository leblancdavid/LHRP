using System;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Runtime
{
    public class ProcessResult
    {
        private List<ProcessResult> _subProcessResult = new List<ProcessResult>();
        public IEnumerable<ProcessResult> SubProcessResult => _subProcessResult;
        public TimeSpan Duration { get; private set; }
        public TimeSpan EstimatedDuration { get; set; }
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }

        public void AppendSubProcessResult(ProcessResult subProcess)
        {
            Duration += subProcess.Duration;
            EstimatedDuration += subProcess.EstimatedDuration;
            _subProcessResult.Add(subProcess);
        }
        
    }
}