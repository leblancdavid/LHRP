using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.LiquidTransfers;

namespace LHRP.Api.Protocol.Steps
{
    public class MultiDispenseLiquidTransferStepData
    {
        public bool PreAliquot { get; private set; }
        private double _preAliquotVolume;
        public double PreAliquotVolume
        {
            get
            {
                return PreAliquot ? _preAliquotVolume : 0.0;
            }
            private set
            {
                _preAliquotVolume = value;
            }
        }
        public bool PostAliquot { get; private set; }
        private double _postAliquotVolume;
        public double PostAliquotVolume
        {
            get
            {
                return PostAliquot ? _postAliquotVolume : 0.0;
            }
            private set
            {
                _postAliquotVolume = value;
            }
        }
        
        public TransferPattern<LiquidToManyTransfer> Pattern { get; private set; }
        public int TipTypeId { get; private set; }
        public bool ReturnTipsToSource { get; private set; }
        public bool ReuseTips { get; private set; }

        public MultiDispenseLiquidTransferStepData(TransferPattern<LiquidToManyTransfer> pattern,
            int tipTypeId,
            bool returnTipsToSource,
            bool reuseTips,
            bool preAliquot,
            double preAliquotVolume,
            bool postAliquot,
            double postAliquotVolume)
        {
            Pattern = pattern;
            TipTypeId = tipTypeId;
            ReuseTips = reuseTips;
            ReturnTipsToSource = returnTipsToSource;
            PreAliquot = preAliquot;
            PreAliquotVolume = preAliquotVolume;
            PostAliquot = postAliquot;
            PostAliquotVolume = postAliquotVolume;
        }
    }
}
