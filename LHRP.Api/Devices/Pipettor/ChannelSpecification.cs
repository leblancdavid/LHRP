using LHRP.Api.Instrument;

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

        public bool CanReach(Coordinates coordinates)
        {
            if(coordinates.X >= LowerReachLimit.X && coordinates.X <= UpperReachLimit.X &&
                coordinates.Y >= LowerReachLimit.Y && coordinates.Y <= UpperReachLimit.Y &&
                coordinates.Z >= LowerReachLimit.Z && coordinates.Z <= UpperReachLimit.Z)
                return true;
            return false;
        }
    }
}