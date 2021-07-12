using System;
using System.Collections.Generic;
using System.Linq;
using LHRP.Api.Runtime.ErrorHandling;

namespace LHRP.Api.Runtime
{
    public class ProcessResult
    {
        public Guid ProcessId { get; private set; }
        public TimeSpan Duration { get; private set; }
        public TimeSpan EstimatedDuration { get; set; }
        public bool ContainsErrors 
        { 
            get
            {
                if(Errors.Count() > 0)
                    return true;
                return false;
            } 
        }
        private List<RuntimeError> _errors = new List<RuntimeError>();
        public IEnumerable<RuntimeError> Errors
        {
            get
            {
                List<RuntimeError> errors = new List<RuntimeError>();
                errors.AddRange(_errors);
                return errors;
            }
        }

        public bool ContainsWarnings 
        { 
            get
            {
                if(Warnings.Count() > 0)
                    return true;
                return false;
            } 
        }
        private List<RuntimeError> _warnings = new List<RuntimeError>();
        public IEnumerable<RuntimeError> Warnings
        {
            get
            {
                List<RuntimeError> warnings = new List<RuntimeError>();
                warnings.AddRange(_warnings);
                return warnings;
            }
        }

        public ProcessResult()
        {
            Duration = new TimeSpan(0);
            EstimatedDuration = new TimeSpan(0);
            ProcessId = Guid.NewGuid();
        }

        public ProcessResult(params RuntimeError[] errors)
        {
            Duration = new TimeSpan(0);
            EstimatedDuration = new TimeSpan(0);
            ProcessId = Guid.NewGuid();
            _errors.AddRange(errors);
        }

        public ProcessResult(TimeSpan estimate, TimeSpan duration)
        {
            Duration = duration;
            EstimatedDuration = estimate;
            ProcessId = Guid.NewGuid();
        }

        public void AddError(RuntimeError error)
        {
            _errors.Add(error);
        }

        public void AddWarning(RuntimeError warning)
        {
            _warnings.Add(warning);
        }

        public void Combine(params ProcessResult[] processes)
        {
            for (int i = 0; i < processes.Length; ++i)
            {
                _errors.AddRange(processes[i].Errors);
                _warnings.AddRange(processes[i].Warnings);
                Duration += processes[i].Duration;
                EstimatedDuration += processes[i].EstimatedDuration;
            }
        }
    }
}