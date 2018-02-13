using System.Collections.Generic;
using LHRP.Api.Devices.Pipettor;

namespace LHRP.Api.Protocol.TransferPattern
{
    public class TransferGroup
    {
        public ChannelPattern ChannelPattern { get; private set; }

        private List<Transfer> _transfers;
        public IEnumerable<Transfer> Transfers => _transfers;

        public TransferGroup(ChannelPattern pattern)
        {
            ChannelPattern = pattern;
        }
    }
}