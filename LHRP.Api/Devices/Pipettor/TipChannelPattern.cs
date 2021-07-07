using System;
using System.Collections.Generic;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Devices.Pipettor
{
    public class TipChannelPattern : ChannelPattern
    {
        private Tip?[] _tips;
        public TipChannelPattern(int numChannels) : base(numChannels)
        {
            _tips = new Tip?[numChannels];
        }

        public Tip? GetTip(int channelIndex)
        {
            if(channelIndex < 0 || channelIndex >= NumChannels)
            {
                throw new Exception($"Invalid channel index {channelIndex}");
            }

            return _tips[channelIndex];
        }

        public void SetTip(int channelIndex, Tip tip)
        {
            if(channelIndex < 0 || channelIndex >= NumChannels)
            {
                throw new Exception($"Invalid channel index {channelIndex}");
            }

            _activeChannels[channelIndex] = true;
            _tips[channelIndex] = tip;
        }

        public void RemoveTip(int channelIndex)
        {
            if(channelIndex < 0 || channelIndex >= NumChannels)
            {
                throw new Exception($"Invalid channel index {channelIndex}");
            }

            _activeChannels[channelIndex] = false;
            _tips[channelIndex] = null;
        }
    }
}