using System;
using System.Collections.Generic;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Devices.Pipettor
{
    public class IndependentChannelSimulatedPipettor: IPipettor, ISimulation
    {
        public SimulationSpeedMode SpeedMode { get; set; }
        public int NumberChannels => throw new NotImplementedException();
        public IEnumerable<ChannelStatus> ChannelStatus => throw new NotImplementedException();
        public IDeviceStatus DeviceStatus { get; }

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

        
        public bool IsInitialized => throw new NotImplementedException();
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