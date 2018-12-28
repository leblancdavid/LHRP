using System.Collections.Generic;
using System.Linq;
using LHRP.Api.CoordinateSystem;

namespace LHRP.Api.Devices.Pipettor
{
    public class PipettorStatus : IDeviceStatus
    {
        public bool HasErrors 
        { 
            get
            {
                return ErrorMessages.Count() > 0;
            }
        }
        private List<string> _errorMessages = new List<string>();
        public IEnumerable<string> ErrorMessages
        {
            get
            {
                var errors = new List<string>();
                errors.AddRange(_errorMessages);
                foreach(var channel in _channelStatus)
                {
                    errors.AddRange(channel.ErrorMessages);
                }
                return errors;
            }
        }
        public Coordinates CurrentPosition { get; set; }
        
        private ChannelStatus[] _channelStatus;
        public IEnumerable<ChannelStatus> ChannelStatus => _channelStatus.ToList();

        public ChannelStatus this[int i]
        {
            get
            {
                return _channelStatus[i];
            }
        }

        public PipettorStatus(int numChannels)
        {
            _channelStatus = new ChannelStatus[numChannels];
            for(int i = 0; i < numChannels; ++i)
            {
                _channelStatus[i] = new ChannelStatus();
            }
            CurrentPosition = new Coordinates();
        }

    }
}