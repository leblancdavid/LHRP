using LHRP.Api.Labware;
using System;

namespace LHRP.Api.Devices.Pipettor
{
    public class TipChannelPattern : ChannelPattern<Tip>
    {
        public TipChannelPattern(int numChannels) : base(numChannels)
        {
        }

        public Tip? GetTip(int channelIndex)
        {
            if(channelIndex < 0 || channelIndex >= NumChannels)
            {
                throw new Exception($"Invalid channel index {channelIndex}");
            }

            return _channels[channelIndex];
        }

        public void SetTip(int channelIndex, Tip tip)
        {
            if(channelIndex < 0 || channelIndex >= NumChannels)
            {
                throw new Exception($"Invalid channel index {channelIndex}");
            }

            _channels[channelIndex] = tip;
        }

        public void RemoveTip(int channelIndex)
        {
            if(channelIndex < 0 || channelIndex >= NumChannels)
            {
                throw new Exception($"Invalid channel index {channelIndex}");
            }

            _channels[channelIndex] = null;
        }
    }
}