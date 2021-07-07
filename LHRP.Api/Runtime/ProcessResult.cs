using System;
using System.Collections.Generic;
using System.Linq;
using LHRP.Api.Runtime.ErrorHandling;

namespace LHRP.Api.Runtime
{
    public class ProcessResult
    {
        public Guid ProcessId { get; private set; }
        private List<ProcessResult> _subProcess = new List<ProcessResult>();
        public IEnumerable<ProcessResult> SubProcess => _subProcess;
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
                _subProcess.ForEach(p => errors.AddRange(p.Errors));
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
                _subProcess.ForEach(p => warnings.AddRange(p.Errors));
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

        public void AppendSubProcess(ProcessResult subProcess)
        {
            Duration += subProcess.Duration;
            EstimatedDuration += subProcess.EstimatedDuration;
            _subProcess.Add(subProcess);
        }
    }
}