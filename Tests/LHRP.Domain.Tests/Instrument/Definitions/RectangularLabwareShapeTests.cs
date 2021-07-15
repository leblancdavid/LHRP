using FluentAssertions;
using LHRP.Api.Instrument;
using LHRP.Api.Labware.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LHRP.Domain.Tests.Instrument.Definitions
{
    public class RectangularLabwareShapeTests
    {
        [Fact]
        public void ClearanceHeight_ShouldBeCorrect()
        {
            var rectangularShape = new RectangularLabwareShape(10.0, 10.0, 10.0, new Coordinates(0.0, 0.0, 0.0));
            rectangularShape.ClearanceHeight.Should().BeApproximately(10.0, 0.00001);
        }

        [Fact]
        public void ClearanceHeight_ShouldBeCorrect_WithDifferentOrigin()
        {
            var rectangularShape = new RectangularLabwareShape(10.0, 10.0, 10.0, new Coordinates(0.0, 0.0, 10.0));
            rectangularShape.ClearanceHeight.Should().BeApproximately(20.0, 0.00001);
        }

        [Fact]
        public void TotalVolume_ShouldBeCorrect()
        {
            var rectangularShape = new RectangularLabwareShape(10.0, 10.0, 10.0, new Coordinates(0.0, 0.0, 0.0));
            rectangularShape.TotalVolume.Should().BeApproximately(1000.0, 0.00001);
        }

        [Fact]
        public void GetHeightAtVolume_ShouldReturnCorrectHeight()
        {
            var rectangularShape = new RectangularLabwareShape(10.0, 10.0, 10.0, new Coordinates(0.0, 0.0, 0.0));
            
            rectangularShape.GetHeightAtVolume(500.0).Should().BeApproximately(5.0, 0.00001);
        }

        [Fact]
        public void GetHeightAtVolume_ShouldReturnCorrectHeight_WithDifferentOrigin()
        {
            var rectangularShape = new RectangularLabwareShape(10.0, 10.0, 10.0, new Coordinates(0.0, 0.0, 5.0));

            rectangularShape.GetHeightAtVolume(500.0).Should().BeApproximately(10.0, 0.00001);
        }
    }
}
