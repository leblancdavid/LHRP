using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.OneToOne;

namespace LHRP.Api.Protocol.Steps
{
    public class TransferSamplesStepData
    {
        public TransferPattern<OneToOneTransfer> Pattern { get; private set; }
        public double DesiredTipSize { get; private set; }
        public bool ReturnTipsToSource { get; private set; }
        public TransferSamplesStepData(TransferPattern<OneToOneTransfer> pattern,
            double desiredTipSize,
            bool returnTipsToSource)
        {
            Pattern = pattern;
            DesiredTipSize = desiredTipSize;
            ReturnTipsToSource = returnTipsToSource;
        }
    }
}