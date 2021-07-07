using LHRP.Api.Labware;

namespace LHRP.Api.Protocol.Transfers
{
    public class TransferTarget : ITransfer
    {
        public LabwareAddress Address { get; private set; }
        public double Volume { get; set; }
        public TransferType TransferType { get; protected set; }

        public TransferTarget(LabwareAddress address, double volume, TransferType transferType)
        {
            this.Address = address;
            this.Volume = volume;
            TransferType = transferType;
        }
    }
}