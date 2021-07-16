using FluentAssertions;
using LHRP.Api.Instrument;
using LHRP.Api.Labware;
using Xunit;

namespace LHRP.Domain.Tests.Labware.Tips
{
    public class TipRackShould
    {
        [Fact]
        public void ConsumeTips()
        {
            var tipRack = new TipRack(
                new TipRackDefinition(300, "", 300, true, 8, 12, new Coordinates(0, 0, 0), 0.0,
                    new RectangularLabwareShape(127.76, 85.48, 55.0)));
            
            tipRack.TotalTipCount.Should().Be(96);
            tipRack.RemainingTips.Should().Be(96);

            var result = tipRack.Consume(new LabwareAddress(1, 1));
            result.IsSuccess.Should().BeTrue();

            tipRack.RemainingTips.Should().Be(95);
        }

        [Fact]
        public void GetNextAvailableTip()
        {
            var tipRack = new TipRack(
                new TipRackDefinition(300, "", 300, true, 8, 12, new Coordinates(0, 0, 0), 0.0,
                    new RectangularLabwareShape(127.76, 85.48, 55.0)), 1);

            var nextTipResult = tipRack.GetNextAvailableTip();

            nextTipResult.IsSuccess.Should().BeTrue();
            nextTipResult.Value.Address
            .Row.Should().Be(1);
            nextTipResult.Value.Address.Column.Should().Be(1);
        }

        [Fact]
        public void Refill()
        {
            var tipRack = new TipRack(
                new TipRackDefinition(300, "", 300, true, 8, 12, new Coordinates(0, 0, 0), 0.0,
                    new RectangularLabwareShape(127.76, 85.48, 55.0)));
            
            tipRack.TotalTipCount.Should().Be(96);
            tipRack.RemainingTips.Should().Be(96);

            var result = tipRack.Consume(new LabwareAddress(1, 1));
            result.IsSuccess.Should().BeTrue();
            tipRack.RemainingTips.Should().Be(95);

            tipRack.Refill();
            tipRack.RemainingTips.Should().Be(96);
        }

        [Fact]
        public void FailToConsumeAnInvalidLabwareAddress()
        {
            var tipRack = new TipRack(
                new TipRackDefinition(300, "",
                    300, true, 8, 12, new Coordinates(0, 0, 0), 0.0,
                    new RectangularLabwareShape(127.76, 85.48, 55.0)));
            var result = tipRack.Consume(new LabwareAddress(9999, 9999));
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void FailToGetTheNextTip_WhenTipRackIsEmpty()
        {
            var tipRack = new TipRack(
                new TipRackDefinition(300, "", 300, true, 1, 1, new Coordinates(0, 0, 0), 0.0,
                    new RectangularLabwareShape(127.76, 85.48, 55.0)));

            var result = tipRack.Consume(new LabwareAddress(1, 1));
            result.IsSuccess.Should().BeTrue();
            tipRack.RemainingTips.Should().Be(0);

            var nextTipResult = tipRack.GetNextAvailableTip();
            nextTipResult.IsFailure.Should().BeTrue();
        }
    }
}