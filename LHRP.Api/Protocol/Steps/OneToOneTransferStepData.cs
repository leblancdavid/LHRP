using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.OneToOne;

namespace LHRP.Api.Protocol.Steps
{
    public class OneToOneTransferStepData
    {
        public TransferPattern<OneToOneTransfer> Pattern { get; private set; }
        public AspirateParameters AspirateParameters { get; private set; }
        public DispenseParameters DispenseParameters { get; private set; }
        public int TipTypeId { get; private set; }
        public bool ReturnTipsToSource { get; private set; }
        public OneToOneTransferStepData(TransferPattern<OneToOneTransfer> pattern,
            AspirateParameters aspirateParameters,
            DispenseParameters dispenseParameters,
            int tipTypeId,
            bool returnTipsToSource)
        {
            Pattern = pattern;
            AspirateParameters = aspirateParameters;
            DispenseParameters = dispenseParameters;
            TipTypeId = tipTypeId;
            ReturnTipsToSource = returnTipsToSource;
        }
    }
}