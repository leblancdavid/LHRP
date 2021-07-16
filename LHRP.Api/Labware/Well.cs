using LHRP.Api.Instrument;

namespace LHRP.Api.Labware
{
    public class Well : LiquidContainer
    {
        public WellDefinition Definition { get; private set; }

        public Well(LabwareAddress address, Coordinates absolutePosition, WellDefinition definition)
            : base(address, absolutePosition, definition.WellShape)
        {
            
            Definition = definition;
        }

        public override LiquidContainer CreateSnapshot()
        {
            var well = new Well(Address, AbsolutePosition, Definition);
            if (IsAssigned)
            {
                well.AssignLiquid(this.Liquid!);
            }
            if (Volume > 0.0)
            {
                well.AddLiquid(this.Liquid!, Volume);
            }
            return well;
        }
    }
}