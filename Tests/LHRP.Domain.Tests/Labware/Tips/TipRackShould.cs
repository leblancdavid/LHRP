using FluentAssertions;
using LHRP.Api.Devices;
using LHRP.Api.Labware;
using LHRP.Api.Labware.Tips;
using Xunit;

namespace LHRP.Domain.Tests.Labware.Tips
{
    public class TipRackShould
    {
        [Fact]
        public void ConsumeTips()
        {
            var tipRack = new TipRack(8, 12, 300, true, 
                new Position(), 9.0);
            
            tipRack.TotalTipCount.Should().Be(96);
            tipRack.RemainingTips.Should().Be(96);

            var result = tipRack.Consume(new LabwareAddress(1, 1));
            result.IsSuccess.Should().BeTrue();

            tipRack.RemainingTips.Should().Be(95);
        }

        [Fact]
        public void GetNextAvailableTip()
        {
            var tipRack = new TipRack(8, 12, 300, true, 
                new Position(), 9.0);

            var nextTipResult = tipRack.GetNextAvailableTip();

            nextTipResult.IsSuccess.Should().BeTrue();
            nextTipResult.Value.Row.Should().Be(1);
            nextTipResult.Value.Column.Should().Be(1);
        }
    }
}