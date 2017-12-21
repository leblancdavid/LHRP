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
                _pipettor.SimulationSpeedFactor = value;
            }
        }
        public double FailureRate { get; set; }
        public NimbusLiteSimulatedInstrument()
        {
            _pipettor = new IndependentChannelSimulatedPipettor();
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