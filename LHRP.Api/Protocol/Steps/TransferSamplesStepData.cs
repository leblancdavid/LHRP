using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.OneToOne;

namespace LHRP.Api.Protocol.Steps
{
    public class TransferSamplesStepData
    {
        public TransferPattern<OneToOneTransfer> Pattern { get; private set; }
        public int TipTypeId { get; private set; }
        public bool ReturnTipsToSource { get; private set; }
        public TransferSamplesStepData(TransferPattern<OneToOneTransfer> pattern,
            int tipTypeId,
            bool returnTipsToSource)
        {
            Pattern = pattern;
            TipTypeId = tipTypeId;
            ReturnTipsToSource = returnTipsToSource;
        }
    }
}