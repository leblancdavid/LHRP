using System.Collections.Generic;
using System.Collections.ObjectModel;
using LHRP.Api.Devices.Pipettor;

namespace LHRP.Api.Protocol.Transfers
{
    public class TransferGroup<T> where T : ITransfer
    {
        public ChannelPattern ChannelPattern { get; private set; }

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
            ChannelPattern = ChannelPattern.Empty(numChannels);
            _transfers = new T[numChannels];
        }
    }
}