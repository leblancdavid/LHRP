using LHRP.Api.Common;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;
using LHRP.Instrument.NimbusLite.Instrument;

namespace LHRP.Instrument.NimbusLite.Runtime
{
    public class NimbusLiteSimulationEngine : IRuntimeEngine, ISimulation, IScheduler
    {
        NimbusLiteSimulatedInstrument _instrument;
        
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
        public NimbusLiteSimulationEngine()
        {
            _instrument = new NimbusLiteSimulatedInstrument();
        }
        public IInstrument Instrument 
        { 
            get
            {
                return _instrument;
            }
        }
        
        public Result<Process> Run(IRunnable run)
        {
            return run.Run(Instrument);
        }

        public Schedule Schedule(IRunnable run)
        {
            SimulationSpeedFactor = 0;
            var process = run.Run(Instrument);
            return new Schedule();
        }
    }
}