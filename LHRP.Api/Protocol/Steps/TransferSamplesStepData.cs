using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Protocol.Steps
{
    public class TransferSamplesStepData
    {
        public TransferPattern Pattern { get; private set; }
        public double DesiredTipSize { get; private set; }
        public bool ReturnTipsToSource { get; private set; }
        public TransferSamplesStepData(TransferPattern pattern,
            double desiredTipSize,
            bool returnTipsToSource)
        {
            Pattern = pattern;
            DesiredTipSize = desiredTipSize;
            ReturnTipsToSource = returnTipsToSource;
        }
    }
}