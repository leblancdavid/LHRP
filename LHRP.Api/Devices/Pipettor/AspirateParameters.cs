using System.Collections.Generic;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Devices.Pipettor
{
    public class AspirateParameters
    {
        private List<TransferTarget> _targets;
        public IEnumerable<TransferTarget> Targets => _targets;
        public ChannelPattern Pattern { get; private set; }
        public  AspirateParameters(List<TransferTarget> targets, ChannelPattern pattern)
        {
            _targets = targets;
            Pattern = pattern;
        }

    }
}