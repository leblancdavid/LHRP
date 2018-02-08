using LHRP.Api.Common;
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

        public abstract Result<Coordinates> GetRealCoordinates(LabwareAddress address);
    }
}