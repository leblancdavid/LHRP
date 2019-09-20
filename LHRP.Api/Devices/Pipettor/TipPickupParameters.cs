namespace LHRP.Api.Devices.Pipettor
{
    public class TipPickupParameters
    {
        public TipChannelPattern Pattern { get; private set; }
        public int TipTypeId { get; private set; }

        public TipPickupParameters(TipChannelPattern pattern, int tipTypeId)
        {
            Pattern = pattern;
            TipTypeId = tipTypeId;
        }
    }
}