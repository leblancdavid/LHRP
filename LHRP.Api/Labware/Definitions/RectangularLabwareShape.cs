using LHRP.Api.Instrument;

namespace LHRP.Api.Labware.Definitions
{
    public class RectangularLabwareShape : ILabwareShape
    {
        public RectangularLabwareShape(double width, double length, double height, Coordinates origin)
        {
            Origin = origin;
            Dimensions = new Dimensions(width, length, height);
            Center = new Coordinates(Origin.X + width / 2.0, Origin.Y + length / 2.0, Origin.Z + height / 2.0);
        }

        public double ClearanceHeight => Origin.Z + Dimensions.Height;

        public double TotalVolume => Dimensions.Width * Dimensions.Length * Dimensions.Height;

        public Coordinates Origin { get; private set; }

        public Dimensions Dimensions { get; private set; }

        public Coordinates Center { get; private set; }

        public double GetHeightAtVolume(double volume)
        {
            if(volume >= TotalVolume)
            {
                return Dimensions.Height;
            }

            return (volume / (Dimensions.Width * Dimensions.Length)) + Origin.Z;
        }
    }
}
