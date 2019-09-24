namespace LHRP.Api.Protocol.Transfers
{
    public class LiquidTarget
    {
        public double Volume { get; private set; }
        public Liquids.Liquid Liquid { get; private set; }

        public TransferType TransferType { get; private set; }

        public LiquidTarget(Liquids.Liquid liquid, double volume, TransferType transferType)
        {
            this.Volume = volume;
            this.Liquid = liquid;
            this.TransferType = transferType;
        }
    }
}
