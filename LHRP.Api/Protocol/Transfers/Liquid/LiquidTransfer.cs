namespace LHRP.Api.Protocol.Transfers.Liquid
{
    public class LiquidTransfer : Transfer
    {
        public Liquids.Liquid Liquid { get; private set; }
        public LiquidTransfer(Liquids.Liquid liquid, TransferTarget target) : base(target)
        {
            Liquid = liquid;
        }
    }
}
