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

        public ProcessResult Aspirate(AspirateParameters parameters)
        {
            Console.WriteLine("Sim: " + parameters);
            return new ProcessResult();
        }

        public ProcessResult Dispense(DispenseParameters parameters)
        {
            Console.WriteLine("Sim: " + parameters);
            return new ProcessResult();
        }

        public ProcessResult PickupTips(TipPickupParameters parameters)
        {
            Console.WriteLine("Sim: " + parameters);
            return new ProcessResult();
        }

        public ProcessResult DropTips(TipDropParameters parameters)
        {
            Console.WriteLine("Sim: " + parameters);
            return new ProcessResult();
        }

        public SimulationSpeedMode SpeedMode { get; set; }
  }
}