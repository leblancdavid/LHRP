using LHRP.Api.Labware;

namespace LHRP.Api.Protocol.Transfers
{
    public class LiquidTransferSource
    {
        public Liquids.Liquid Liquid { get; private set; }
        public double Volume { get; set; }

        public LiquidTransferSource(Liquids.Liquid liquid, double volume)
        {
            this.Volume = volume;
            this.Liquid = liquid;
        }
    }
}
