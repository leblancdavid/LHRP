using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.OneToOne;

namespace LHRP.Api.Protocol.Steps
{
    public class OneToOneTransferStepData
    {
        public TransferPattern<OneToOneTransfer> Pattern { get; private set; }
        public int TipTypeId { get; private set; }
        public bool ReturnTipsToSource { get; private set; }
        public OneToOneTransferStepData(TransferPattern<OneToOneTransfer> pattern,
            int tipTypeId,
            bool returnTipsToSource)
        {
            Pattern = pattern;
            TipTypeId = tipTypeId;
            ReturnTipsToSource = returnTipsToSource;
        }
    }
}