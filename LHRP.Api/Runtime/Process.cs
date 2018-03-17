using System;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Runtime
{
    public class Process
    {
        public Guid ProcessId { get; private set; }
        private List<Process> _subProcess = new List<Process>();
        public IEnumerable<Process> SubProcess => _subProcess;
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
        private List<string> _errors = new List<string>();
        public IEnumerable<string> Errors
        {
            get
            {
                List<string> errors = new List<string>();
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
        private List<string> _warnings = new List<string>();
        public IEnumerable<string> Warnings
        {
            get
            {
                List<string> warnings = new List<string>();
                warnings.AddRange(_warnings);
                _subProcess.ForEach(p => warnings.AddRange(p.Errors));
                return warnings;
            }
        }

        public Process()
        {
            Duration = new TimeSpan(0);
            EstimatedDuration = new TimeSpan(0);
            ProcessId = Guid.NewGuid();
        }

        public Process(TimeSpan estimate, TimeSpan duration)
        {
            Duration = duration;
            EstimatedDuration = estimate;
            ProcessId = Guid.NewGuid();
        }

        public void AddError(string error)
        {
            _errors.Add($"({ProcessId.ToString()}): {error}");
        }

        public void AddWarning(string warning)
        {
            _warnings.Add($"({ProcessId.ToString()}): {warning}");
        }

        public void AppendSubProcess(Process subProcess)
        {
            Duration += subProcess.Duration;
            EstimatedDuration += subProcess.EstimatedDuration;
            _subProcess.Add(subProcess);
        }
    }
}