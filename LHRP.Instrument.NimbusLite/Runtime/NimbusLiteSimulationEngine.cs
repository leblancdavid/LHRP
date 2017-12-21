using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;
using LHRP.Instrument.NimbusLite.Instrument;

namespace LHRP.Instrument.NimbusLite.Runtime
{
    public class NimbusLiteSimulationEngine : IRuntimeEngine, ISimulation, IScheduler
    {
        NimbusLiteSimulatedInstrument _instrument;
        public NimbusLiteSimulationEngine()
        {
            _instrument = new NimbusLiteSimulatedInstrument();
            SpeedMode = SimulationSpeedMode.RealTime;
        }
        public IInstrument Instrument 
        { 
            get
            {
                return _instrument;
            }
        }
        
        private SimulationSpeedMode _speedMode;
        public SimulationSpeedMode SpeedMode 
        { 
            get
            {
                return _speedMode;
            }
            set
            {
                _speedMode = value;
                _instrument.SpeedMode = _speedMode;
            }
        }

        public ProcessResult Run(IRunnable run)
        {
            return run.Run(Instrument);
        }

        public Schedule Schedule()
        {
            return new Schedule();
        }
    }
}