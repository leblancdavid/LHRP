using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Devices.Pipettor
{
    public class IndependentChannelSimulatedPipettor: IPipettor, ISimulation
    {
    
        public IndependentChannelSimulatedPipettor()
        {

        }

        public void Aspirate(AspirateParameters parameters)
        {
            
        }

        public void Dispense(DispenseParameters parameters)
        {

        }

        public void PickupTips(TipPickupParameters parameters)
        {
        }

        public void DropTips(TipDropParameters parameters)
        {
            
        }

        public SimulationSpeedMode SpeedMode { get; set; }
  }
}