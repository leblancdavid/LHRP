using LHRP.Api.Labware;
using LHRP.Api.Liquids;

namespace LHRP.Api.Protocol.Transfers
{
    public class LiquidTransferTarget : TransferTarget
    {
        public Liquid Liquid { get; private set; }
        
        public LiquidTransferTarget(LabwareAddress address, int positionId, Liquid liquid, double volume) 
            : base(address, positionId, volume)
        {
            Liquid = liquid;
        }

    }
}