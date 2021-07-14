using LHRP.Api.Devices;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Labware
{
    public class Tip : IStateSnapshotGetter<Tip>
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

        public Tip GetSnapshot()
        {
            return new Tip(Address, AbsolutePosition, TipVolume, IsFiltered, TipTypeId);
        }
    }
}