using LHRP.Api.CoordinateSystem;

namespace LHRP.Api.Devices.Pipettor
{
    public class TipDropParameters
    {
        public TipChannelPattern Pattern { get; private set; }
        public bool ReturnToSource { get; private set; }
        public Coordinates WastePosition { get;  private set; }

        public TipDropParameters(Coordinates wastePosition)
        {
            WastePosition = wastePosition;
            ReturnToSource = false;
        }

        public TipDropParameters(TipChannelPattern pattern)
        {
            Pattern = pattern;
            ReturnToSource = true;
        }
    }
}