using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;
using LHRP.Instrument.SimplePipettor.Instrument;

namespace LHRP.Instrument.SimplePipettor.Runtime
{
    public class SimplePipettorSimulationEngine : IRuntimeEngine, ISimulation, IScheduler
    {
        SimplePipettorSimulatedInstrument _instrument;
        
        private uint _simulationSpeedFactor;
        public uint SimulationSpeedFactor 
        { 
            get
            {
                return _simulationSpeedFactor;
            }
            set
            {
                _simulationSpeedFactor = value;
                _instrument.SimulationSpeedFactor = value;
            }
        }
        public double FailureRate { get; set; }
        public SimplePipettorSimulationEngine()
        {
            _instrument = new SimplePipettorSimulatedInstrument();
        }
        public IInstrument Instrument 
        { 
            get
            {
                return _instrument;
            }
        }
        
        public Process Run(IRunnable run)
        {
            return run.Run(this);
        }

        public Process Schedule(IRunnable run)
        {
            var previousSpeedFactor = _simulationSpeedFactor;
            SimulationSpeedFactor = 0;
            var process = run.Run(this);
            //reset the speed
            SimulationSpeedFactor = previousSpeedFactor;

            return process;
        }
    }
}