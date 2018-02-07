using LHRP.Api.Labware;

namespace LHRP.Api.Protocol.TransferPattern
{
    public class TransferTarget
    {
        public TransferTarget(int positionId, LabwareAddress address, double volume)
        {
            PositionId = positionId;
            Address = address;
            Volume = volume;
        }
        public LabwareAddress Address { get; private set; }
        public int PositionId { get; private set; }
        public double Volume { get; private set; }
    }
}