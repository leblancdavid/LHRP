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
    }
}