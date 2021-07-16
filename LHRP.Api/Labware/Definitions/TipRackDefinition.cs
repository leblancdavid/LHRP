using LHRP.Api.Instrument;

namespace LHRP.Api.Labware
{
    public class TipRackDefinition : ILabwareDefinition
    {
        public int Id { get; private set; }
        public string DisplayName { get; private set; }
        public double TipVolume { get; private set; }
        public bool AreFilteredTips { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public Coordinates Offset { get; private set; }
        public double Spacing { get; private set; }

        public ILabwareShape Shape { get; private set; }

        public TipRackDefinition(int id,
            string displayName,
            double tipVolume,
            bool areFilteredTips, int rows, int columns, 
            Coordinates offset, double spacing,
            ILabwareShape shape)
        {
            Id = id;
            DisplayName = displayName;
            TipVolume = tipVolume;
            AreFilteredTips = areFilteredTips;
            Rows = rows;
            Columns = columns;
            Offset = offset;
            Spacing = spacing;
            Shape = shape;
        }

        public Labware CreateInstance(int instanceId)
        {
            return new TipRack(this, instanceId);
        }
    }
}