using System;
using System.Collections.Generic;
using LHRP.Api.Common;
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

        public Result<Process> Aspirate(AspirateParameters parameters)
        {
            Console.WriteLine(parameters);
            return Result<Process>.Ok(new Process(new TimeSpan(), new TimeSpan()));
        }

        public Result<Process> Dispense(DispenseParameters parameters)
        {
            Console.WriteLine(parameters);
            return Result<Process>.Ok(new Process(new TimeSpan(), new TimeSpan()));
        }

        public Result<Process> PickupTips(TipPickupParameters parameters)
        {
            Console.WriteLine(parameters);
            return Result<Process>.Ok(new Process(new TimeSpan(), new TimeSpan()));
        }

        public Result<Process> DropTips(TipDropParameters parameters)
        {
            Console.WriteLine(parameters);
            return Result<Process>.Ok(new Process(new TimeSpan(), new TimeSpan()));
        }

        public Result<Process> Initialize()
        {
        throw new NotImplementedException();
        }

        public Result<Process> Deinitialize()
        {
        throw new NotImplementedException();
        }
  }
}