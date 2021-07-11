using LHRP.Api.Devices.Pipettor;

namespace LHRP.Api.Protocol.Transfers.OneToOne
{
    public static class OneToOneChannelPatternExtensions
    {
        public static ChannelPattern<TransferTarget> ToSourceTransfer(
            this ChannelPattern<OneToOneTransfer> channelPattern)
        {
            var output = new ChannelPattern<TransferTarget>(channelPattern.NumChannels);
            for(int i = 0; i < channelPattern.NumChannels; ++i)
            {
                if (channelPattern[i] != null)
                {
                    output[i] = channelPattern[i]!.Source;
                }
            }

            return output;
        }

        public static ChannelPattern<TransferTarget> ToTargetTransfer(
            this ChannelPattern<OneToOneTransfer> channelPattern)
        {
            var output = new ChannelPattern<TransferTarget>(channelPattern.NumChannels);
            for (int i = 0; i < channelPattern.NumChannels; ++i)
            {
                if (channelPattern[i] != null)
                {
                    output[i] = channelPattern[i]!.Target;
                }
            }

            return output;
        }
    }
}