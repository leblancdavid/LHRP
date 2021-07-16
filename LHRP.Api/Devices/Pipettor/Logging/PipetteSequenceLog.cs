using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Devices.Pipettor
{
    public class PipetteSequenceLog
    {
        private List<ChannelPipettingTransfer> _transfers = new List<ChannelPipettingTransfer>();
        public IEnumerable<ChannelPipettingTransfer> Transfers => _transfers;
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public PipetteSequenceLog()
        {
        }

        public PipetteSequenceLog(List<ChannelPipettingTransfer> transfers, DateTime startTime, DateTime endTime)
        {
            _transfers = transfers;
            StartTime = startTime;
            EndTime = endTime;
        }

        public bool HasTransferedLiquid(Liquid liquid)
        {
            return _transfers.Any(x => x.Liquid != null && x.Liquid.ContainsLiquid(liquid));
        }

        public bool HasTransferedFrom(LabwareAddress address)
        {
            return _transfers.Any(x => x.Container.Address == address && x.Transfer == TransferType.Aspirate);
        }

        public bool HasTransferedTo(LabwareAddress address)
        {
            return _transfers.Any(x => x.Container.Address == address && x.Transfer == TransferType.Dispense);
        }

        public void Add(ChannelPipettingTransfer transfer)
        {
            if(!_transfers.Any())
            {
                StartTime = DateTime.UtcNow;
            }
            _transfers.Add(transfer);
            EndTime = DateTime.UtcNow;
        }

        public static PipetteSequenceLog Combine(params PipetteSequenceLog[] sequences)
        {
            var ordered = sequences.OrderBy(x => x.StartTime).ToList();
            if (!ordered.Any())
                return new PipetteSequenceLog();

            var transfers = new List<ChannelPipettingTransfer>();
            foreach (var sequence in ordered)
            {
                transfers.AddRange(sequence.Transfers);
            }

            return new PipetteSequenceLog(transfers, ordered.First().StartTime, ordered.Last().EndTime);
        }
    }
}
