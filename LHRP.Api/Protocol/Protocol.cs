using System.Collections.Generic;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol
{
    public class Protocol
    {
        private List<IStep> _steps;
        public IEnumerable<IStep> Steps => _steps;
        void AddStep(IStep step)
        {
            _steps.Add(step);
        }

        void Run(IInstrument instrument)
        {
            instrument.ExecutionMode = ExecutionMode.Execution;
            foreach(var step in _steps)
            {
                step.Run(instrument);
            }
        }

        void Simulate(IInstrument instrument)
        {
            instrument.ExecutionMode = ExecutionMode.Simulation;
            foreach(var step in _steps)
            {
                step.Run(instrument);
            }
        }

        void Schedule(IInstrument instrument)
        {
            instrument.ExecutionMode = ExecutionMode.Scheduling;
            foreach(var step in _steps)
            {
                step.Run(instrument);
            }
        }
         
    }
}