using LHRP.Api.Labware;

namespace LHRP.Api.Protocol.Transfers
{
    public class LiquidSource
    {
        public Liquids.Liquid Liquid { get; private set; }
        public double Volume { get; set; }

        public LiquidSource(Liquids.Liquid liquid, double volume)
        {
            this.Volume = volume;
            this.Liquid = liquid;
        }
    }
}
