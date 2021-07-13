using LHRP.Api.Liquids;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Devices.Pipettor
{
    public class LiquidTransferLog
    {
        private List<LiquidTransferContainerLog> _containers = new List<LiquidTransferContainerLog>();
        public IEnumerable<LiquidTransferContainerLog> Containers => _containers;
        public IEnumerable<LiquidTransferLogEntry> Entries
        {
            get
            {
                var entries = new List<LiquidTransferLogEntry>();
                foreach(var container in _containers)
                {
                    entries.AddRange(container.Entries);
                }
                return entries.OrderBy(x => x.EntryTime);
            }
        }

        public Liquid Liquid { get; private set; }

        public LiquidTransferLog(Liquid liquid)
        {
            Liquid = liquid;
        }

        public void AddEntry(LiquidTransferLogEntry entry)
        {
            if(!entry.Liquid.Match(Liquid))
            {
                return;
            }

            var container = _containers.FirstOrDefault(x => x.Address == entry.Address);
            if (container == null)
            {
                container = new LiquidTransferContainerLog(Liquid, entry.Address);
                _containers.Add(container);
            }

            container.AddEntry(entry);
        }
    }
}
