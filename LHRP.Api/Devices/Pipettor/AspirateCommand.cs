using LHRP.Api.CoordinateSystem;

namespace LHRP.Api.Devices.Pipettor
{
    public class AspirateCommand
    {
        public double Volume { get; set; }
        public Coordinates Position { get; set; }
        
    }
}