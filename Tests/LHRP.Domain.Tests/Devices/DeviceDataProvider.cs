using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices.Pipettor;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Domain.Tests.Devices
{
    public static class DeviceDataProvider
    {
        public static IPipettor GetSimulatedIndependentChannelPipettor()
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

            var specs = new PipettorSpecification(channelSpecification,
                new Coordinates(0.0, 9.0, 0.0),
                true);

            return new DefaultSimulatedPipettor(specs);
        }
    }
}
