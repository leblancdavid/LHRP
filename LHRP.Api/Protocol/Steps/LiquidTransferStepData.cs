using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.LiquidTransfers;

namespace LHRP.Api.Protocol.Steps
{
    public class LiquidTransferStepData
    {
        public Liquids.Liquid Liquid { get; private set; }
        public TransferPattern<LiquidToOneTransfer> Pattern { get; private set; }
        public int TipTypeId { get; private set; }
        public bool ReturnTipsToSource { get; private set; }
        public bool ReuseTips { get; private set; }

        public LiquidTransferStepData(TransferPattern<LiquidToOneTransfer> pattern,
            Liquids.Liquid liquid,
            int tipTypeId,
            bool returnTipsToSource,
            bool reuseTips)
        {
            Pattern = pattern;
            Liquid = liquid;
            TipTypeId = tipTypeId;
            ReuseTips = reuseTips;
            ReturnTipsToSource = returnTipsToSource;
        }
    }
}
