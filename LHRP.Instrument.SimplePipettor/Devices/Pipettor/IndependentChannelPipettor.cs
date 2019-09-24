using System;
using System.Collections.Generic;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime;

namespace LHRP.Instrument.SimplePipettor.Devices.Pipettor
{
    public class IndependentChannelPipettor : IPipettor
    {
        public Guid DeviceId { get; private set; }
        public int NumberChannels { get; private set; }
        public PipettorStatus PipettorStatus { get; private set; }
        public IDeviceStatus DeviceStatus { get; }
        public bool IsInitialized => throw new NotImplementedException();

        public PipettorSpecification Specification { get; private set; }

        public IndependentChannelPipettor()
        {
            var channelSpecification = new List<ChannelSpecification>();
            //Add two channels that can reach anywhere on the deck for now.
            channelSpecification.Add(new ChannelSpecification(
                new Coordinates(double.MinValue, double.MinValue, double.MinValue),
                new Coordinates(double.MaxValue, double.MaxValue, double.MaxValue)
            )); 
            channelSpecification.Add(new ChannelSpecification(
                new Coordinates(double.MinValue, double.MinValue, double.MinValue),
                new Coordinates(double.MaxValue, double.MaxValue, double.MaxValue)
            ));

            Specification = new PipettorSpecification(channelSpecification,
                new Coordinates(0.0, 9.0, 0.0),
                true); 
        }

        public ProcessResult Aspirate(AspirateParameters parameters,
            List<TransferTarget> targets,
            ChannelPattern pattern)
        {
            Console.WriteLine(parameters);
            return new ProcessResult(new TimeSpan(), new TimeSpan());
        }

        public ProcessResult Dispense(DispenseParameters parameters,
            List<TransferTarget> targets,
            ChannelPattern pattern)
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