using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Devices.Pipettor
{
    public class LiquidTransferContainerLog
    {
        private List<LiquidTransferLogEntry> _entries = new List<LiquidTransferLogEntry>();
        public IEnumerable<LiquidTransferLogEntry> Entries => _entries;
        public Liquid Liquid { get; private set; }
        public LabwareAddress Address { get; private set; }

        public LiquidTransferContainerLog(Liquid liquid, LabwareAddress address)
        {
            Liquid = liquid;
            Address = address;
        }

        public void AddEntry(LiquidTransferLogEntry entry)
        {
            if(!entry.Liquid.Match(Liquid) || entry.Address != Address)
            {
                return;
            }

            if(entry.Transfer == TransferType.Aspirate && 
                (!_entries.Any(x => x.Transfer == TransferType.Dispense && x.Address == entry.Address) ||
                _entries.Any(x => x.Transfer == TransferType.Aspirate && x.Address == entry.Address && x.IsOriginalSource)))
            {
                entry.IsOriginalSource = true;
            }

            _entries.Add(entry);
        }

        public bool IsSource()
        {
            if(!_entries.Any() || _entries.First().Transfer == TransferType.Dispense)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<LabwareAddress> GetDestinations()
        {
            return _entries.Where(x => x.Transfer == TransferType.Dispense).Select(x => x.Address).Distinct();
        }

    }
}
