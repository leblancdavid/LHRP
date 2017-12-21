using System;
using System.Collections.Generic;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Devices.Pipettor
{
    public class IndependentChannelPipettor : IPipettor
    {
        public int NumberChannels { get; private set; }
        public IDeviceStatus DeviceStatus { get; }
        public bool IsInitialized => throw new NotImplementedException();

        public IndependentChannelPipettor()
        {
            
        }

        public ProcessResult Aspirate(AspirateParameters parameters)
        {
            Console.WriteLine(parameters);
            return new ProcessResult(new TimeSpan(), new TimeSpan());
        }

        public ProcessResult Dispense(DispenseParameters parameters)
        {
            Console.WriteLine(parameters);
            return new ProcessResult(new TimeSpan(), new TimeSpan());
        }

        public ProcessResult PickupTips(TipPickupParameters parameters)
        {
            Console.WriteLine(parameters);
            return new ProcessResult(new TimeSpan(), new TimeSpan());
        }

        public ProcessResult DropTips(TipDropParameters parameters)
        {
            Console.WriteLine(parameters);
            return new ProcessResult(new TimeSpan(), new TimeSpan());
        }

        public ProcessResult Initialize()
        {
        throw new NotImplementedException();
        }

        public ProcessResult Deinitialize()
        {
        throw new NotImplementedException();
        }
  }
}