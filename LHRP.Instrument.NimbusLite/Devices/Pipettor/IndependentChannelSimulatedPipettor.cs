using System;
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
            Console.WriteLine("Sim: " + parameters);
        }

        public void Dispense(DispenseParameters parameters)
        {
            Console.WriteLine("Sim: " + parameters);
        }

        public void PickupTips(TipPickupParameters parameters)
        {
            Console.WriteLine("Sim: " + parameters);
        }

        public void DropTips(TipDropParameters parameters)
        {
            Console.WriteLine("Sim: " + parameters);
        }

        public SimulationSpeedMode SpeedMode { get; set; }
  }
}