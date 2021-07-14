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
    public class PipetteSequenceLogTests
    {
        [Fact]
        public void ShouldBeAbleToAddTransfers()
        {
            var sequence = new PipetteSequenceLog();
            sequence.Add(new ChannelPipettingTransfer(33, 1, new Liquid("Test"), new Coordinates(),
                new LabwareAddress(1, 1), TransferType.Aspirate));

            sequence.Transfers.Count().Should().Be(1);
        }

        [Fact]
        public void HasTransferedLiquid_ShouldReturnTrue_WhenLiquidIsPresent()
        {
            var sequence = new PipetteSequenceLog();
            var testLiquid = new Liquid("Test");
            sequence.Add(new ChannelPipettingTransfer(33, 1, testLiquid, new Coordinates(),
                new LabwareAddress(1, 1), TransferType.Aspirate));

            sequence.HasTransferedLiquid(testLiquid).Should().BeTrue();
        }

        [Fact]
        public void HasTransferedLiquid_ShouldReturnFalse_WhenLiquidIsNotPresent()
        {
            var sequence = new PipetteSequenceLog();
            var testLiquid1 = new Liquid("Test1");
            var testLiquid2 = new Liquid("Test2");
            sequence.Add(new ChannelPipettingTransfer(33, 1, testLiquid1, new Coordinates(),
                new LabwareAddress(1, 1), TransferType.Aspirate));

            sequence.HasTransferedLiquid(testLiquid2).Should().BeFalse();
        }

        [Fact]
        public void HasTransferedFrom_ShouldReturnTrue_WhenAddressIsPresent()
        {
            var sequence = new PipetteSequenceLog();
            var testLiquid = new Liquid("Test");
            var testAddress = new LabwareAddress(1, 1, 1);
            sequence.Add(new ChannelPipettingTransfer(33, 1, testLiquid, new Coordinates(),
                testAddress, TransferType.Aspirate));

            sequence.HasTransferedFrom(testAddress).Should().BeTrue();
        }

        [Fact]
        public void HasTransferedFrom_ShouldReturnFalse_WhenAddressIsNotPresent()
        {
            var sequence = new PipetteSequenceLog();
            var testLiquid = new Liquid("Test");
            var testAddress1 = new LabwareAddress(1, 1, 1);
            var testAddress2 = new LabwareAddress(1, 33, 1);
            sequence.Add(new ChannelPipettingTransfer(33, 1, testLiquid, new Coordinates(),
                testAddress1, TransferType.Aspirate));

            sequence.HasTransferedFrom(testAddress2).Should().BeFalse();
        }

        [Fact]
        public void HasTransferedTo_ShouldReturnTrue_WhenAddressIsPresent()
        {
            var sequence = new PipetteSequenceLog();
            var testLiquid = new Liquid("Test");
            var testAddress = new LabwareAddress(1, 1, 1);
            sequence.Add(new ChannelPipettingTransfer(33, 1, testLiquid, new Coordinates(),
                testAddress, TransferType.Dispense));

            sequence.HasTransferedTo(testAddress).Should().BeTrue();
        }

        [Fact]
        public void HasTransferedTo_ShouldReturnFalse_WhenAddressIsNotPresent()
        {
            var sequence = new PipetteSequenceLog();
            var testLiquid = new Liquid("Test");
            var testAddress1 = new LabwareAddress(1, 1, 1);
            var testAddress2 = new LabwareAddress(1, 33, 1);
            sequence.Add(new ChannelPipettingTransfer(33, 1, testLiquid, new Coordinates(),
                testAddress1, TransferType.Dispense));

            sequence.HasTransferedTo(testAddress2).Should().BeFalse();
        }
    }
}
