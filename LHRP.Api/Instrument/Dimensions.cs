namespace LHRP.Api.Instrument
{
    public class Dimensions
    {
        public double Width {get; private set;}
        public double Height {get; private set;}
        public double Length {get; private set;}

        public Dimensions(double width, double length, double height)
        {
            Width = width;
            Height = height;
            Length = length;
        }
    }
}