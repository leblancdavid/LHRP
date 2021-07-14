using FluentAssertions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;
using System.Linq;
using Xunit;

namespace LHRP.Domain.Tests.Devices.Pipettor.Logging
{
    public class InMemoryPipetteLoggerTests
    {
        [Fact]
        public void ShouldCompleteATransferSequence()
        {
            var logger = new InMemoryPipetteLogger();
            logger.BeginSequence(ChannelPattern.Full(1));
            logger.LogTransfer(new ChannelPattern<ChannelPipettingTransfer>(new ChannelPipettingTransfer[]
            {
                new ChannelPipettingTransfer(33, 0, new Liquid("Test"), new Coordinates(), new LabwareAddress(1, 1, 1), TransferType.Aspirate)
            }));
            logger.LogTransfer(new ChannelPattern<ChannelPipettingTransfer>(new ChannelPipettingTransfer[]
            {
                new ChannelPipettingTransfer(33, 0, new Liquid("Test"), new Coordinates(), new LabwareAddress(1, 1, 2), TransferType.Dispense)
            }));
            logger.EndSequence(ChannelPattern.Full(1));

            logger.Sequences.Count().Should().Be(1);
        }
    }
}
