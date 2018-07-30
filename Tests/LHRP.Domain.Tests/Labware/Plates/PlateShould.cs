using FluentAssertions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Labware;
using LHRP.Api.Labware.Plates;
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
        public void SuccessfullyGetRealCoordinatesOfLabwareAddress()
        {
            var result = _plate96.GetRealCoordinates(new LabwareAddress(1, 1));
            result.IsSuccess.Should().BeTrue();
        }

        public void FailToGetRealCoordinates_OfInvalidLabwareAddresses()
        {
            var result = _plate96.GetRealCoordinates(new LabwareAddress(9999, 9999));
            result.IsSuccess.Should().BeFalse();
        }
    }
}