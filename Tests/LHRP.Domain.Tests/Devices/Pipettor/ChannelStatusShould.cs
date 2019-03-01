using FluentAssertions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware;
using LHRP.Api.Labware.Tips;
using Xunit;

namespace LHRP.Domain.Tests.Devices.Pipettor
{
    public class ChannelStatusShould
    {
        [Fact]
        public void UpdateTipStatusOnTipPickup()
        {
            var channelStatus = new ChannelStatus();
            channelStatus.OnPickedUpTip(new Tip(
                new LabwareAddress(1,1,-1), 
                new Coordinates(1,1,1),
                50, false, 0));
            
            channelStatus.HasTip.Should().BeTrue();
            channelStatus.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void HaveErrorsWhenAttemptingToPickupTipsOnAnOccuppiedChannel()
        {
            var channelStatus = new ChannelStatus();
            channelStatus.OnPickedUpTip(new Tip(
                new LabwareAddress(1,1,-1), 
                new Coordinates(1,1,1),
                50, false, 0));
            
            channelStatus.OnPickedUpTip(new Tip(
                new LabwareAddress(2,2,-1), 
                new Coordinates(2,2,2),
                50, false, 0));

            channelStatus.HasErrors.Should().BeTrue();
        }

        [Fact]
        public void UpdateVolumeWhenAspirating()
        {
            var channelStatus = new ChannelStatus();
            channelStatus.OnPickedUpTip(new Tip(
                new LabwareAddress(1,1,-1), 
                new Coordinates(1,1,1),
                50, false, 0));

            channelStatus.OnAspiratedVolume(20);

            channelStatus.CurrentVolume.Should().Be(20);
            channelStatus.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void HaveErrorsWhenAspiratingMoreThanTipCapacity()
        {
            var channelStatus = new ChannelStatus();
            channelStatus.OnPickedUpTip(new Tip(
                new LabwareAddress(1,1,-1), 
                new Coordinates(1,1,1),
                50, false, 0));

            channelStatus.OnAspiratedVolume(999);

            channelStatus.HasErrors.Should().BeTrue();
        }

        [Fact]
        public void HaveErrorsWhenAspiratingWhenNoTipIsPresent()
        {
            var channelStatus = new ChannelStatus();
            channelStatus.OnAspiratedVolume(999);
            channelStatus.HasErrors.Should().BeTrue();
        }

        [Fact]
        public void UpdateVolumeWhenAspiratingAndDispensing()
        {
            var channelStatus = new ChannelStatus();
            channelStatus.OnPickedUpTip(new Tip(
                new LabwareAddress(1,1,-1), 
                new Coordinates(1,1,1),
                50, false, 0));

            channelStatus.OnAspiratedVolume(50);
            channelStatus.OnDispensedVolume(25);

            channelStatus.CurrentVolume.Should().Be(25);
            channelStatus.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void HaveErrorsWhenDispensingWhenNoTipIsPresent()
        {
            var channelStatus = new ChannelStatus();
            channelStatus.OnDispensedVolume(999);
            channelStatus.HasErrors.Should().BeTrue();
        }

        [Fact]
        public void NotHaveTipsAfterDroppingTips()
        {
            var channelStatus = new ChannelStatus();
            channelStatus.OnPickedUpTip(new Tip(
                new LabwareAddress(1,1,-1), 
                new Coordinates(1,1,1),
                50, false, 0));

            channelStatus.OnDroppedTip();

            channelStatus.HasTip.Should().BeFalse();
            channelStatus.HasErrors.Should().BeFalse();
        }


        
    }
}