using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;

namespace LHRP.Api.Labware
{
    public abstract class Labware
    {
        public string Name { get; protected set; }
        public double Width { get; protected set; }
        public double Height { get; protected set; }
        public double Depth { get; protected set; }

        protected int _positionId;
        public int PositionId 
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

        public abstract Result<Coordinates> GetRealCoordinates(LabwareAddress address);
        public void UpdatePosition(Coordinates position, int positionId)
        {
            AbsolutePosition = position;
            PositionId = positionId;
        }

    }
}