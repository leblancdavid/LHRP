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
        public IEnumerable<ChannelStatus> ChannelStatus => throw new NotImplementedException();
        public IDeviceStatus DeviceStatus { get; }
        public bool IsInitialized => throw new NotImplementedException();

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

        public void Initialize()
        {
        throw new NotImplementedException();
        }

        public void Deinitialize()
        {
        throw new NotImplementedException();
        }
  }
}