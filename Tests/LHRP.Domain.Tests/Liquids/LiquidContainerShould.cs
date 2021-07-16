using System.Linq;
using FluentAssertions;
using LHRP.Api.Instrument;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using Xunit;

namespace LHRP.Domain.Tests.Liquids
{
    public class LiquidContainerShould
    {
        private LiquidContainer _liquidContainer;

        public LiquidContainerShould()
        {
            _liquidContainer = new LiquidContainer(new LabwareAddress(1,1), new Coordinates(0,0 ,0), 
                new RectangularLabwareShape(100.0, 100.0, 100, new Coordinates()));
        }

        [Fact]
        public void BeEmpty_WhenNoLiquidHasBeenAdded()
        {
            _liquidContainer.IsAssigned.Should().BeFalse();
        }

        [Fact]
        public void NotBeEmpty_WhenLiquidHasBeenAdded()
        {
            _liquidContainer.AddLiquid(new Liquid(LiquidType.Water), 100);
            _liquidContainer.IsAssigned.Should().BeTrue();
        }

        [Fact]
        public void SuccessfullyAddLiquid()
        {
            var liquid = new Liquid(LiquidType.Water);
            _liquidContainer.AddLiquid(liquid, 100);
            _liquidContainer.Liquid.Should().NotBeNull();
            _liquidContainer.Liquid.LiquidParts.Count().Should().Be(1);
            _liquidContainer.ContainsLiquid(liquid).Should().BeTrue();
            _liquidContainer.Volume.Should().Be(100);
            //should also not contain any other liquid
            _liquidContainer.ContainsLiquid(new Liquid(LiquidType.Water)).Should().BeFalse();
        }

        [Fact]
        public void SuccessfullyAddMultipleLiquids()
        {
            var liquid1 = new Liquid(LiquidType.Water);
            var liquid2 = new Liquid(LiquidType.Water);
            _liquidContainer.AddLiquid(liquid1, 100);
            _liquidContainer.AddLiquid(liquid2, 100);
            _liquidContainer.Liquid.Should().NotBeNull();
            _liquidContainer.Liquid.LiquidParts.Count().Should().Be(2);
            _liquidContainer.ContainsLiquid(liquid1).Should().BeTrue();
            _liquidContainer.ContainsLiquid(liquid2).Should().BeTrue();
            _liquidContainer.Volume.Should().Be(200);

            //should also not contain any other liquid
            _liquidContainer.ContainsLiquid(new Liquid(LiquidType.Water)).Should().BeFalse();
        }

        [Fact]
        public void SuccessfullyRemoveVolumeOfLiquid()
        {
            var liquid = new Liquid(LiquidType.Water);
            _liquidContainer.AddLiquid(liquid, 100);
            _liquidContainer.Volume.Should().Be(100);

            _liquidContainer.Remove(50);
            _liquidContainer.Volume.Should().Be(50);
        }

        
    }
}