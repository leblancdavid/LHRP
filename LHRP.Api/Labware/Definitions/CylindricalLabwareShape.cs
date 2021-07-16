using LHRP.Api.Instrument;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Labware
{
    public class CylindricalLabwareShape : ILabwareShape
    {
        public CylindricalLabwareShape(double radius, double height)
        {
            Origin = new Coordinates();
            Dimensions = new Dimensions(radius, radius, height);
            Center = new Coordinates(Origin.X + radius, Origin.Y + radius, Origin.Z + height / 2.0);
        }

        public CylindricalLabwareShape(double radius, double height, Coordinates origin)
        {
            Origin = origin;
            Dimensions = new Dimensions(radius, radius, height);
            Center = new Coordinates(Origin.X + radius, Origin.Y + radius, Origin.Z + height / 2.0);
        }

        public double ClearanceHeight => Origin.Z + Dimensions.Height;

        public double TotalVolume => Math.PI * Dimensions.Width * Dimensions.Width * Dimensions.Height;

        public Coordinates Origin { get; private set; }

        public Dimensions Dimensions { get; private set; }
        public Coordinates Center { get; private set; }

        public double GetHeightAtVolume(double volume)
        {
            if (volume >= TotalVolume)
            {
                return Dimensions.Height;
            }

            return (volume / (Math.PI * Dimensions.Width * Dimensions.Width)) + Origin.Z;
        }
    }
}
