using LHRP.Api.CoordinateSystem;

namespace LHRP.Api.Devices.Pipettor
{
    public class ChannelSpecification
    {
        public Coordinates LowerReachLimit { get; private set; }
        public Coordinates UpperReachLimit { get; private set; }

        public ChannelSpecification(Coordinates lowerReach,
            Coordinates upperReach)
        {
            LowerReachLimit = lowerReach;
            UpperReachLimit = upperReach;
        }
    }
}