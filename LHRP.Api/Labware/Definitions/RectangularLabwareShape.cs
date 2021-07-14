using LHRP.Api.Instrument;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Labware.Definitions
{
    public class RectangularLabwareShape : ILabwareShape
    {
        public RectangularLabwareShape(double width, double length, double height, Coordinates origin)
        {
            Origin = origin;
            Dimensions = new Dimensions(width, length, height);
        }

        public double ClearanceHeight => Origin.Z + Dimensions.Height;

        public double TotalVolume => Dimensions.Width * Dimensions.Length * Dimensions.Height;

        public Coordinates Origin { get; private set; }

        public Dimensions Dimensions { get; private set; }
    }
}
