using System.Collections.Generic;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol
{
    public class Protocol : IRunnable
    {
        private List<IRunnable> _steps = new List<IRunnable>();
        public IEnumerable<IRunnable> Steps => _steps;
        public void AddStep(IRunnable step)
        {
            _steps.Add(step);
        }

        public void Run(IInstrument instrument)
        {
            foreach(var step in _steps)
            {
                step.Run(instrument);
            }
        }
    }
}