
using LHRP.Api.Instrument;

namespace LHRP.Api.Labware
{
    public class PlateDefinition : ILabwareDefinition
    {
        public int Id { get; private set; }
        public string DisplayName { get; private set; }
        public WellDefinition WellDefinition { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public Coordinates Offset { get; private set; }
        public double Spacing { get; private set; }
        public ILabwareShape Shape { get; private set; }

        public PlateDefinition(string displayName,
            ILabwareShape shape,
            WellDefinition wellDefinition,
            int rows, int columns, 
            Coordinates offset, double spacing)
        {
            Id = 0;
            DisplayName = displayName;
            Shape = shape;
            WellDefinition = wellDefinition;
            Rows = rows;
            Columns = columns;
            Offset = offset;
            Spacing = spacing;
        }

        public Labware CreateInstance(int instanceId)
        {
            return new Plate(this, instanceId);
        }
    }
}