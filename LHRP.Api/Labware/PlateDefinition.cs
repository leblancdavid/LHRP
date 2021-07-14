
using LHRP.Api.Instrument;

namespace LHRP.Api.Labware
{
    public class PlateDefinition
    {
        public int Id { get; private set; }
        public string DisplayName { get; private set; }
        public WellDefinition WellDefinition { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public Coordinates Offset { get; private set; }
        public double Spacing { get; private set; }

        public PlateDefinition(string displayName,
            WellDefinition wellDefinition,
            int rows, int columns, 
            Coordinates offset, double spacing)
        {
            Id = 0;
            DisplayName = displayName;
            WellDefinition = wellDefinition;
            Rows = rows;
            Columns = columns;
            Offset = offset;
            Spacing = spacing;
        }
    }
}