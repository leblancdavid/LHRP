using System.Collections.Generic;
using System.Collections.ObjectModel;
using LHRP.Api.Devices.Pipettor;

namespace LHRP.Api.Protocol.Transfers
{
    public class TransferGroup<T> where T : ITransfer
    {
        private ChannelPattern _channelPattern;
        public ChannelPattern ChannelPattern
        {
            get
            {
                return _channelPattern;
            }
            set
            {

                _channelPattern = value;
                for(int i = 0; i < _channelPattern.NumChannels && i < _transfers.Length; ++i)
                {
                    if(!_channelPattern[i])
                    {
                        _transfers[i] = default(T);
                    }
                }
            }
        }

        private T[] _transfers;
        public T[] Transfers => _transfers;
    
        public T this[int i]
        {
            get
            {
                return _transfers[i];
            }
            set
            {
                if (value == null)
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
            _channelPattern = ChannelPattern.Empty(numChannels);
            _transfers = new T[numChannels];
        }
    }
}