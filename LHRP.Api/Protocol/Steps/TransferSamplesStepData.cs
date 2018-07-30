using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Protocol.Steps
{
    public class TransferSamplesStepData
    {
        public TransferPattern<Transfer> Pattern { get; private set; }
        public double DesiredTipSize { get; private set; }
        public bool ReturnTipsToSource { get; private set; }
        public TransferSamplesStepData(TransferPattern<Transfer> pattern,
            double desiredTipSize,
            bool returnTipsToSource)
        {
            Pattern = pattern;
            DesiredTipSize = desiredTipSize;
            ReturnTipsToSource = returnTipsToSource;
        }
    }
}