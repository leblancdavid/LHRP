using System.Collections.Generic;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Devices.Pipettor
{
    public class DispenseParameters
    {
        private List<TransferTarget> _targets;
        public IEnumerable<TransferTarget> Targets => _targets;
        public ChannelPattern Pattern { get; private set; }
        public  DispenseParameters(List<TransferTarget> targets, ChannelPattern pattern)
        {
            _targets = targets;
            Pattern = pattern;
        }
    }
}