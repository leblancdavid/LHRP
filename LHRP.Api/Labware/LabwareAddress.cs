using LHRP.Api.Devices;

namespace LHRP.Api.Labware
{
  public class LabwareAddress
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public LabwareAddress(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public bool Equals(LabwareAddress other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Row == Row && other.Column == Column;
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
                return result;
            }
        }
    }
}