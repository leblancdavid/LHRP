using LHRP.Api.Instrument;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Labware.Definitions
{
    public class CylindricalLabwareShape : ILabwareShape
    {
        public CylindricalLabwareShape(double radius, double height, Coordinates origin)
        {
            Origin = origin;
            Dimensions = new Dimensions(radius, radius, height);
        }

        public double ClearanceHeight => Origin.Z + Dimensions.Height;

        public double TotalVolume => Math.PI * Dimensions.Width * Dimensions.Width * Dimensions.Height;

        public Coordinates Origin { get; private set; }

        public Dimensions Dimensions { get; private set; }
    }
}
