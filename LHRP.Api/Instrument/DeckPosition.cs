using CSharpFunctionalExtensions;
using LHRP.Api.Runtime;

namespace LHRP.Api.Instrument
{
    public class DeckPosition : ISnapshotCreator<DeckPosition>
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
        public Labware.Labware? AssignedLabware { get; private set; }

        public bool IsOccupied
        {
            get
            {
                return AssignedLabware != null;
            }
        }

        public Result Assign(Labware.Labware labware)
        {
            AssignedLabware = labware;
            AssignedLabware.UpdatePosition(Position);
            return Result.Ok();
        }

        public DeckPosition CreateSnapshot()
        {
            var snapshotPosition = new DeckPosition(PositionId, Dimensions, Position);
            if (this.IsOccupied)
                snapshotPosition.Assign(this.AssignedLabware!.CreateSnapshot());

            return snapshotPosition;
        }

        public void RemoveLabware()
        {
            AssignedLabware = null;
        }

    }
}