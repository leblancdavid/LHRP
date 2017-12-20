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

        public ProcessResult Aspirate(AspirateParameters parameters)
        {
            Console.WriteLine(parameters);
            return new ProcessResult();
        }

        public ProcessResult Dispense(DispenseParameters parameters)
        {
            Console.WriteLine(parameters);
            return new ProcessResult();
        }

        public ProcessResult PickupTips(TipPickupParameters parameters)
        {
            Console.WriteLine(parameters);
            return new ProcessResult();
        }

        public ProcessResult DropTips(TipDropParameters parameters)
        {
            Console.WriteLine(parameters);
            return new ProcessResult();
        }
  }
}