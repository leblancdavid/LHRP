using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;

namespace LHRP.Api.Labware.Tips
{
    public class TipRackDefinition
    {
        public int Id { get; private set; }
        public string DisplayName { get; private set; }
        public double TipVolume { get; private set; }
        public bool AreFilteredTips { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public Coordinates Offset { get; private set; }
        public double Spacing { get; private set; }

        public TipRackDefinition(string displayName, 
            Dimensions dimensions,
            double tipVolume,
            bool areFilteredTips, int rows, int columns, 
            Coordinates offset, double spacing)
        {
            Id = 0;
            DisplayName = displayName;
            TipVolume = tipVolume;
            AreFilteredTips = areFilteredTips;
            Rows = rows;
            Columns = columns;
            Offset = offset;
            Spacing = spacing;
        }

    }
}