using CSharpFunctionalExtensions;
using LHRP.Api.Devices;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Labware
{
    public abstract class Labware : ISnapshotCreator<Labware>
    {
        public string Name { get; protected set; }
        public double Width { get; protected set; }
        public double Height { get; protected set; }
        public double Depth { get; protected set; }

        protected int _instanceId;
        public virtual int InstanceId 
        { 
            get
            {
                return _instanceId;
            } 
            set
            {
                _instanceId = value;
            } 
        }
        protected Coordinates _absolutePosition = new Coordinates();
        public virtual Coordinates AbsolutePosition 
        { 
            get
            {
                return _absolutePosition;
            } 
            set
            {
                _absolutePosition = value;
            }
        }

        public Labware()
        {
            Name = "";
        }

        public abstract Coordinates? GetRealCoordinates(LabwareAddress address);
        public void UpdatePosition(Coordinates position)
        {
            AbsolutePosition = position;
        }

        public abstract Labware CreateSnapshot();
    }
}