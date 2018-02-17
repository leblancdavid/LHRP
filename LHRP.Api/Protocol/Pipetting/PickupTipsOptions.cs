using LHRP.Api.Devices.Pipettor;

namespace LHRP.Api.Protocol.Pipetting
{
    public class PickupTipsOptions
    {
        public ChannelPattern Pattern { get; private set; }
        public double DesiredTipSize { get; private set; }

        public PickupTipsOptions(ChannelPattern pattern, double tipSize)
        {
            Pattern = pattern;
            DesiredTipSize = tipSize;
        }
        
    }
}