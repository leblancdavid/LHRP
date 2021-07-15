using FluentAssertions;
using LHRP.Api.Instrument;
using LHRP.Api.Labware;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LHRP.Domain.Tests.Instrument.Definitions
{
    public class CylindricalLabwareShapeTests
    {
        [Fact]
        public void ClearanceHeight_ShouldBeCorrect()
        {
            var shape = new CylindricalLabwareShape(10.0, 10.0, new Coordinates(0.0, 0.0, 0.0));
            shape.ClearanceHeight.Should().BeApproximately(10.0, 0.00001);
        }

        [Fact]
        public void ClearanceHeight_ShouldBeCorrect_WithDifferentOrigin()
        {
            var shape = new CylindricalLabwareShape(10.0, 10.0, new Coordinates(0.0, 0.0, 10.0));
            shape.ClearanceHeight.Should().BeApproximately(20.0, 0.00001);
        }

        [Fact]
        public void TotalVolume_ShouldBeCorrect()
        {
            var shape = new CylindricalLabwareShape(10.0, 10.0, new Coordinates(0.0, 0.0, 0.0));
            var expectedVolume = Math.PI * 1000.0;
            shape.TotalVolume.Should().BeApproximately(expectedVolume, 0.00001);
        }

        [Fact]
        public void GetHeightAtVolume_ShouldReturnCorrectHeight()
        {
            var shape = new CylindricalLabwareShape(10.0, 10.0, new Coordinates(0.0, 0.0, 0.0));
            var expectedVolume = Math.PI * 1000.0;
            shape.GetHeightAtVolume(expectedVolume / 2.0).Should().BeApproximately(5.0, 0.00001);
        }

        [Fact]
        public void GetHeightAtVolume_ShouldReturnCorrectHeight_WithDifferentOrigin()
        {
            var shape = new CylindricalLabwareShape(10.0, 10.0, new Coordinates(0.0, 0.0, 5.0));
            var expectedVolume = Math.PI * 1000.0;
            shape.GetHeightAtVolume(expectedVolume / 2.0).Should().BeApproximately(10.0, 0.00001);
        }
    }
}
