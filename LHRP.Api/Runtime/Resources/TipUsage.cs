using LHRP.Api.Instrument;

namespace LHRP.Api.Runtime.Resources
{
    public class TipUsage
    {
        public int TipTypeId { get; private set; }
        public int ExpectedTotalTipUsage { get; set; }

        public TipUsage(int tipTypeId)
        {
            TipTypeId = tipTypeId;
        }

        public int GetExpectedReloadCount(IInstrument instrument)
        {
            return 0;
        }
    }
}
