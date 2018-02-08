using LHRP.Api.Common;
using LHRP.Api.Devices;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Instrument
{
    public class DeckPosition
    {
        public DeckPosition(int positionId, 
            Coordinates dimensions, 
            Coordinates position)
        {
            this.PositionId = positionId;
            this.Dimensions = dimensions;
            this.Position = position;
        }
        public int PositionId { get; private set; }
        public Coordinates Dimensions { get; private set; }
        public Coordinates Position { get; private set; }
        public Labware.Labware AssignedLabware { get; private set; }

        public bool IsOccupied 
        {
            get
            {
                return AssignedLabware != null;
            }
        }

        public bool ContainsTipRack
        {
            get
            {
                return AssignedLabware != null && AssignedLabware is TipRack;
            }
        }

        public Result Assign(Labware.Labware labware)
        {
            AssignedLabware = labware;
            return Result.Ok();
        }

        public void RemoveLabware()
        {
            AssignedLabware = null;
        }

    }
}