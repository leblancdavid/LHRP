using LHRP.Api.Devices;

namespace LHRP.Api.Labware
{
  public class LabwareAddress
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public int PositionId { get; private set; }
        public Position AbsolutePosition { get; private set; }

        public LabwareAddress(int row, int column, int positionId, Position absolutePosition)
        {
            Row = row;
            Column = column;
            PositionId = positionId;
            AbsolutePosition = absolutePosition;
        }

        public bool Equals(LabwareAddress other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Row == Row && other.Column == Column && other.PositionId == PositionId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (LabwareAddress)) return false;
            return Equals((LabwareAddress) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Row;
                result = (result*397) ^ Column;
                result = (result*397) ^ PositionId;
                return result;
            }
        }
    }
}