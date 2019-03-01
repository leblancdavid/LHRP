using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;

namespace LHRP.Api.Labware.Tips
{
    public class Tip
    {
        public LabwareAddress Address { get; private set; }
        public Coordinates AbsolutePosition { get; private set; }
        public bool IsFiltered { get; private set; }
        public double TipVolume { get; private set; }
        public int TipTypeId { get; private set; }

        public Tip(LabwareAddress address, Coordinates absolutePosition, double tipVolume, bool filtered, int tipTypeId)
        {
            Address = address;
            AbsolutePosition = absolutePosition;
            IsFiltered = filtered;
            TipVolume = tipVolume;
            TipTypeId = tipTypeId;
        }
    }
}