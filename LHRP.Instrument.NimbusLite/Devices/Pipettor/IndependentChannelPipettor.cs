using System;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Devices.Pipettor
{
    public class IndependentChannelPipettor : IPipettor
    {
        public IndependentChannelPipettor()
        {
            
        }

        public void Aspirate(AspirateParameters parameters)
        {
            Console.WriteLine(parameters);
        }

        public void Dispense(DispenseParameters parameters)
        {
            Console.WriteLine(parameters);
        }

        public void PickupTips(TipPickupParameters parameters)
        {
            Console.WriteLine(parameters);
        }

        public void DropTips(TipDropParameters parameters)
        {
            Console.WriteLine(parameters);
        }
  }
}