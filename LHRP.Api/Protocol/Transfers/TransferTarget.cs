using LHRP.Api.Labware;

namespace LHRP.Api.Protocol.Transfers
{
    public class TransferTarget : LiquidTarget
    {
        public LabwareAddress Address { get; private set; }

        public TransferTarget(LabwareAddress address, Liquids.Liquid liquid, double volume, TransferType transferType)
            :base(liquid, volume, transferType)
        {
            this.Address = address;
        }
    }
}