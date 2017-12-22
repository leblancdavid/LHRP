using LHRP.Api.Devices;

namespace LHRP.Api.Labware.Tips
{
    public class Tip
    {
        public LabwareAddress Address { get; private set; }
        public Position RelativePosition { get; private set; }
        public bool IsFiltered { get; private set; }
        public double TipVolume { get; private set; }

        public Tip(LabwareAddress address, Position relativePosition, double tipVolume, bool filtered)
        {
            Address = address;
            RelativePosition = relativePosition;
            IsFiltered = filtered;
            TipVolume = tipVolume;
        }
    }
}