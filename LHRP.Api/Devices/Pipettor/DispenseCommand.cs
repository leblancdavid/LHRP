using LHRP.Api.CoordinateSystem;

namespace LHRP.Api.Devices.Pipettor
{
    public class DispenseCommand
    {
        public double Volume { get; set; }
        public Coordinates Position { get; set; }
    }
}