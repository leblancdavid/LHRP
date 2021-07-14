using LHRP.Api.Instrument;
using System.Collections.Generic;

namespace LHRP.Api.Devices.Pipettor
{
    public class PipettorSpecification
    {
        public Coordinates ChannelSpacing { get; private set; } 
        public bool AreIndependentChannels { get; private set; }

        private List<ChannelSpecification> _channels;
        public IEnumerable<ChannelSpecification> Channels => _channels;

        public int NumChannels 
        {
            get 
            {
                return _channels.Count;
            }
        }

        public ChannelSpecification this[int i]
        {
            get { return _channels[i]; }
            private set { _channels[i] = value; }
        }

        public PipettorSpecification(List<ChannelSpecification> channels, 
            Coordinates channelSpacing,
            bool independent)
        {
            _channels = channels;
            ChannelSpacing = channelSpacing;
            AreIndependentChannels = independent;
        }

        
    }
}