using System.Collections.Generic;

namespace LHRP.Api.Devices.Pipettor
{
    public class PipettorStatus : IDeviceStatus
    {
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public Coordinates CurrentPosition { get; set; }
        
        private List<ChannelStatus> _channelStatus;
        public IEnumerable<ChannelStatus> ChannelStatus => _channelStatus;

        public PipettorStatus(int numChannels)
        {
            _channelStatus = new List<ChannelStatus>();
            for(int i = 0; i < numChannels; ++i)
            {
                _channelStatus.Add(new ChannelStatus());
            }
            CurrentPosition = new Coordinates();
            HasError = false;
            ErrorMessage = string.Empty;
        }

    }
}