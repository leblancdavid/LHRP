using System.Collections.Generic;
using LHRP.Api.Devices.Pipettor;

namespace LHRP.Api.Protocol.Transfers
{
    public class TransferGroup<T> where T : Transfer
    {
        public ChannelPattern ChannelPattern { get; private set; }

        private List<T> _transfers;
        public IEnumerable<T> Transfers => _transfers;

        public T this[int i]
        {
            get 
            { 
                return _transfers[i]; 
            }
            set 
            {
                if(value == null)
                {
                    ChannelPattern[i] = false;
                }
                else
                {
                    ChannelPattern[i] = true;
                }
                _transfers[i] = value; 
            }
        }
        public TransferGroup(int numChannels)
        {
            ChannelPattern = ChannelPattern.Empty(numChannels);
            _transfers = new List<T>(numChannels);
        }
    }
}