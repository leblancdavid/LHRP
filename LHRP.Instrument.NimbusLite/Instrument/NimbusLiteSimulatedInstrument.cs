using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;
using LHRP.Instrument.NimbusLite.Devices.Pipettor;

namespace LHRP.Instrument.NimbusLite.Instrument
{
    public class NimbusLiteSimulatedInstrument : IInstrument, ISimulation
    {
        IndependentChannelSimulatedPipettor _pipettor;
        public NimbusLiteSimulatedInstrument()
        {
            _pipettor = new IndependentChannelSimulatedPipettor();
            SpeedMode = SimulationSpeedMode.RealTime;
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
                _pipettor.SpeedMode = _speedMode;
            }
        }

        public IDevice GetDevice(int id)
        {
        throw new System.NotImplementedException();
        }

        public IPipettor GetPipettor()
        {
            return _pipettor;
        }
    }
}