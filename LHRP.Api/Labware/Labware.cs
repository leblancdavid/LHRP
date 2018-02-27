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
        public int PositionId { get; protected set; }
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