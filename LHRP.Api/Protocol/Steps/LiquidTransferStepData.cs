using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.Liquid;

namespace LHRP.Api.Protocol.Steps
{
    public class LiquidTransferStepData
    {
        public bool PreAliquot { get; set; }
        public double PreAliquotVolume { get; set; }
        public bool PostAliquot { get; set; }
        public double PostAliquotVolume { get; set; }
        public bool MultiDispense { get; set; }
        public TransferPattern<LiquidTransfer> Pattern { get; set; }

    }
}
