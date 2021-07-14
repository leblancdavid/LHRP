namespace LHRP.Api.Instrument
{
    public class Dimensions
    {
        public double Width {get; private set;}
        public double Height {get; private set;}
        public double Depth {get; private set;}

        public Dimensions(double width, double height, double depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }
    }
}