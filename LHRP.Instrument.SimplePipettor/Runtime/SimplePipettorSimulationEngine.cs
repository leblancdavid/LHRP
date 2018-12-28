using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Errors;
using LHRP.Instrument.SimplePipettor.Instrument;

namespace LHRP.Instrument.SimplePipettor.Runtime
{
    public class SimplePipettorSimulationEngine : BaseRuntimeEngine, IRuntimeEngine, ISimulation, IScheduler
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
            :base(new SimplePipettorSimulatedInstrument(), new RuntimeCommandQueue(), new DefaultErrorHandler())
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