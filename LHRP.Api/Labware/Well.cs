
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Liquids;

namespace LHRP.Api.Labware
{
    public class Well : LiquidContainer
    {
        public WellDefinition Definition { get; private set; }

        public Well(LabwareAddress address, Coordinates absolutePosition, WellDefinition definition)
            : base(address, absolutePosition, definition.WellCapacity)
        {
            
            Definition = definition;
        }
    }
}