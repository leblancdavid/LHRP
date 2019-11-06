using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Protocol.Steps
{
    public class LiquidTransferStepData
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
        public bool MultiDispense
        {
            get
            {
                return PreAliquot || PostAliquot;
            }
        }

        public Liquids.Liquid Liquid { get; private set; }
        public TransferPattern<Transfer> Pattern { get; private set; }
        public int TipTypeId { get; private set; }
        public bool ReturnTipsToSource { get; private set; }
        public bool ReuseTips { get; private set; }

        public LiquidTransferStepData(TransferPattern<Transfer> pattern,
            Liquids.Liquid liquid,
            int tipTypeId,
            bool returnTipsToSource,
            bool reuseTips,
            bool preAliquot,
            double preAliquotVolume,
            bool postAliquot,
            double postAliquotVolume)
        {
            Pattern = pattern;
            Liquid = liquid;
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
