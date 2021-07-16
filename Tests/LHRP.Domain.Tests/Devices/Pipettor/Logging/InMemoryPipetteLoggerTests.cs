using FluentAssertions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
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
            var logger = GetPrePopulatedTestLog1();
            logger.Sequences.Count().Should().Be(1);
        }

        [Fact]
        public void Reset_ShouldClearLogger()
        {
            var logger = GetPrePopulatedTestLog1();
            logger.Reset();
            logger.Sequences.Count().Should().Be(0);
        }

        [Fact]
        public void GetSourceTransfers_ShouldReturnSources()
        {
            var logger = GetPrePopulatedTestLog1();
            var sourceAddress = new LabwareAddress(1, 1, 1);
            
            var sources = logger.GetSourceTransfers().ToList();
            sources.Count().Should().Be(1);
            sources[0].Container.Address.Should().Be(sourceAddress);

        }

        [Fact]
        public void GetLiquidTracking_ShouldReturnValidSequence_WhenProvidingValidAddress()
        {
            var logger = GetPrePopulatedTestLog1();
            var sourceAddress = new LabwareAddress(1, 1, 1);

            var sequence = logger.GetLiquidTracking(address: sourceAddress);
            sequence.Transfers.Should().NotBeEmpty();
        }

        [Fact]
        public void GetLiquidTracking_ShouldReturnValidSequence_WhenProvidingValidLiquid()
        {
            var logger = GetPrePopulatedTestLog1();
            var sourceLiquid = new Liquid("Test");

            var sequence = logger.GetLiquidTracking(liquid: sourceLiquid);
            sequence.Transfers.Should().NotBeEmpty();
        }

        [Fact]
        public void GetLiquidTracking_ShouldReturnNull_WhenProvidingAnInvalidAddress()
        {
            var logger = GetPrePopulatedTestLog1();
            var sourceAddress = new LabwareAddress(222, 222, 222);

            var sequence = logger.GetLiquidTracking(address: sourceAddress);
            sequence.Should().BeNull();
        }

        [Fact]
        public void GetLiquidTracking_ShouldReturnNull_WhenProvidingAnInvalidLiquid()
        {
            var logger = GetPrePopulatedTestLog1();
            var sourceLiquid = new Liquid("DefinitelyNotTheRightLiquid");

            var sequence = logger.GetLiquidTracking(liquid: sourceLiquid);
            sequence.Should().BeNull();
        }

        private InMemoryPipetteLogger GetPrePopulatedTestLog1()
        {
            var logger = new InMemoryPipetteLogger();
            var sourceAddress = new LabwareAddress(1, 1, 1);
            logger.BeginSequence(ChannelPattern.Full(1));
            logger.LogTransfer(new ChannelPattern<ChannelPipettingTransfer>(new ChannelPipettingTransfer[]
            {
                new ChannelPipettingTransfer(33,
                   new Liquid("Test"),
                   new AspirateParameters(),
                   1,
                   GetTestContainer(sourceAddress),
                   TransferType.Aspirate)
            }));
            logger.LogTransfer(new ChannelPattern<ChannelPipettingTransfer>(new ChannelPipettingTransfer[]
            {
                new ChannelPipettingTransfer(33,
                   new Liquid("Test"),
                   new DispenseParameters(),
                   1,
                   GetTestContainer(new LabwareAddress(1, 1, 2)),
                   TransferType.Dispense)
            }));
            logger.EndSequence(ChannelPattern.Full(1));
            return logger;
        }

        private LiquidContainer GetTestContainer(LabwareAddress address)
        {
            return new LiquidContainer(address, new Coordinates(), new RectangularLabwareShape(10.0, 10.0, 10.0));
        }

    }
}
