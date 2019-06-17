using LHRP.Api.Labware;
using LHRP.Api.Liquids;

namespace LHRP.Api.Protocol.Transfers
{
    public class TransferTarget
    {
        public LabwareAddress Address { get; private set; }
        public double Volume { get; private set; }
        public Liquid Liquid { get; private set; }
    
        public TransferType TransferType { get; private set; }

        public TransferTarget(LabwareAddress address, Liquid liquid, double volume, TransferType transferType)
        {
            this.Address = address;
            this.Volume = volume;
            this.Liquid = liquid;
            this.TransferType = transferType;
        }
    }
}