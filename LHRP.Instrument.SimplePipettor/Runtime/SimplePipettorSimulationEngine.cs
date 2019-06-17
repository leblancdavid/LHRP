using LHRP.Api.Runtime;
using LHRP.Instrument.SimplePipettor.Instrument;
using LHRP.Instrument.SimplePipettor.Runtime.ErrorHandling;

namespace LHRP.Instrument.SimplePipettor.Runtime
{
    public class SimplePipettorSimulationEngine : BaseRuntimeEngine, IRuntimeEngine, ISimulation
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
            :base(new SimplePipettorSimulatedInstrument(), new RuntimeCommandQueue(), new SimplePipettorErrorHandler())
        {
            _instrument = Instrument as SimplePipettorSimulatedInstrument;
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