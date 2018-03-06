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
                new LabwareAddress(1,1), 
                new Coordinates(1,1,1),
                50, false));
            
            channelStatus.HasTip.Should().BeTrue();
            channelStatus.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void HaveErrorsWhenAttemptingToPickupTipsOnAnOccuppiedChannel()
        {
            var channelStatus = new ChannelStatus();
            channelStatus.OnPickedUpTip(new Tip(
                new LabwareAddress(1,1), 
                new Coordinates(1,1,1),
                50, false));
            
            channelStatus.OnPickedUpTip(new Tip(
                new LabwareAddress(2,2), 
                new Coordinates(2,2,2),
                50, false));

            channelStatus.HasErrors.Should().BeTrue();
        }

        [Fact]
        public void UpdateVolumeWhenAspirating()
        {
            var channelStatus = new ChannelStatus();
            channelStatus.OnPickedUpTip(new Tip(
                new LabwareAddress(1,1), 
                new Coordinates(1,1,1),
                50, false));

            channelStatus.OnAspiratedVolume(20);

            channelStatus.CurrentVolume.Should().Be(20);
            channelStatus.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void HaveErrorsWhenAspiratingMoreThanTipCapacity()
        {
            var channelStatus = new ChannelStatus();
            channelStatus.OnPickedUpTip(new Tip(
                new LabwareAddress(1,1), 
                new Coordinates(1,1,1),
                50, false));

            channelStatus.OnAspiratedVolume(999);

            channelStatus.HasErrors.Should().BeTrue();
        }

        
    }
}