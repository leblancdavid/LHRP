using LHRP.Api.Devices;

namespace LHRP.Api.Labware.Tips
{
    public class Tip
    {
        public LabwareAddress Address { get; private set; }
        public Position AbsolutePosition { get; private set; }
        public bool IsFiltered { get; private set; }
        public double TipVolume { get; private set; }

        public Tip(LabwareAddress address, Position absolutePosition, double tipVolume, bool filtered)
        {
            Address = address;
            AbsolutePosition = absolutePosition;
            IsFiltered = filtered;
            TipVolume = tipVolume;
        }
    }
}