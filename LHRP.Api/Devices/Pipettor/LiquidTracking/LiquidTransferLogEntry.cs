using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;
using System;

namespace LHRP.Api.Devices.Pipettor
{
    public class LiquidTransferLogEntry
    {
        public Liquid Liquid { get; private set; }
        public LabwareAddress Address { get; private set; }
        public double Volume { get; private set; }
        public TransferType Transfer { get; private set; }
        public bool IsOriginalSource { get; set; }
        public DateTime EntryTime { get; private set; }

        public LiquidTransferLogEntry(Liquid liquid, LabwareAddress address, double volume, TransferType transfer, bool isOriginalSource = false)
        {
            Liquid = liquid;
            Address = address;
            Volume = volume;
            Transfer = transfer;
            IsOriginalSource = isOriginalSource;
            EntryTime = DateTime.UtcNow;
        }
    }
}
