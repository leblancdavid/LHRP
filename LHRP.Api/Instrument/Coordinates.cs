namespace LHRP.Api.Instrument
{
    public class Coordinates
    {
        public double X { get;  set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Coordinates(double x, double y, double z) 
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Coordinates()
        {
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
        }
    }
}