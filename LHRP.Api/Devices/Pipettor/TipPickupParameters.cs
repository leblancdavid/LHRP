namespace LHRP.Api.Devices.Pipettor
{
    public class TipPickupParameters
    {
        public TipChannelPattern Pattern { get; private set; }

        public TipPickupParameters(TipChannelPattern pattern)
        {
            Pattern = pattern;
        }
    }
}