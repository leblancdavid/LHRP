namespace LHRP.Api.Instrument
{
    public class Dimensions
    {
        public double Width {get; private set;}
        public double Height {get; private set;}
        public double Length {get; private set;}

        public Dimensions()
        {
            Width = 0;
            Height = 0;
            Length = 0;
        }

        public Dimensions(double width, double length, double height)
        {
            Width = width;
            Height = height;
            Length = length;
        }
    }
}