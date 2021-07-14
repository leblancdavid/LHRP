using System.Linq;
using FluentAssertions;
using LHRP.Api.Instrument;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using Xunit;

namespace LHRP.Domain.Tests.Labware.Plates
{
    public class PlateShould
    {
        private Plate _plate96;
        public PlateShould()
        {
            _plate96 = new Plate(new PlateDefinition("Plate96", new WellDefinition(), 8, 12, new Coordinates(85.0, 127.0, 14.5), 9.0));
        }

        [Fact]
        public void SuccessfullyGetRealCoordinates_OfLabwareAddress()
        {
            var result = _plate96.GetRealCoordinates(new LabwareAddress(1, 1,-1));
            (result != null).Should().BeTrue();
        }

        [Fact]
        public void FailToGetRealCoordinates_OfInvalidLabwareAddresses()
        {
            var result = _plate96.GetRealCoordinates(new LabwareAddress(9999, 9999,-1));
            (result != null).Should().BeFalse();
        }

        [Fact]
        public void SuccessfullyRetrieveAWell_GivenALabwareAddress()
        {
            var result = _plate96.GetWell(new LabwareAddress(1, 2,-1));
            (result != null).Should().BeTrue();
            result.Address.Row.Should().Be(1);
            result.Address.Column.Should().Be(2);
        }

        [Fact]
        public void FailToGetAWell_GivenAnInvalidLabwareAddress()
        {
            var result = _plate96.GetWell(new LabwareAddress(9999, 9999,-1));
            (result != null).Should().BeFalse();
        }

        [Fact]
        public void SuccessfullyGetAWell_GivenALiquid()
        {
            var well = _plate96.GetWell(new LabwareAddress(1, 2,-1));
            (well != null).Should().BeTrue();
            var liquid = new Liquid(LiquidType.Water);
            well.AddLiquid(liquid, 100.0);

            var wellsWithLiquid = _plate96.GetWellsWithLiquid(liquid);
            wellsWithLiquid.Count().Should().Be(1);
            var foundWell = wellsWithLiquid.First();
            foundWell.Address.Row.Should().Be(1);
            foundWell.Address.Column.Should().Be(2);
        }

        [Fact]
        public void NotFindAnyWells_WhenNoLiquidHasBeenAdded()
        {
            var liquid = new Liquid(LiquidType.Water);
            var wellsWithLiquid = _plate96.GetWellsWithLiquid(liquid);
            wellsWithLiquid.Count().Should().Be(0);
        }

    }
}