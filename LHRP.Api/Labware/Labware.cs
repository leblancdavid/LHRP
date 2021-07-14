using CSharpFunctionalExtensions;
using LHRP.Api.Devices;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Labware
{
    public abstract class Labware : IStateSnapshotGetter<Labware>
    {
        public string Name { get; protected set; }
        public double Width { get; protected set; }
        public double Height { get; protected set; }
        public double Depth { get; protected set; }

        protected int _positionId;
        public virtual int PositionId 
        { 
            get
            {
                return _positionId;
            } 
            protected set
            {
                _positionId = value;
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
        public void UpdatePosition(Coordinates position, int positionId)
        {
            AbsolutePosition = position;
            PositionId = positionId;
        }

        public abstract Labware GetSnapshot();
    }
}