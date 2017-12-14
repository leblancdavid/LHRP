using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Devices.Pipettor
{
    public class PickupTipsCommand : Command
    {
        public PickupTipsCommand(string channelPattern,
            int position) : base("PickupTips")
        {
            SetValue("ChannelPattern", channelPattern);
            SetValue("Position", position);
        }
    }
}