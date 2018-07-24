
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Liquids;

namespace LHRP.Api.Labware.Plates
{
    public class Well : LiquidContainer
    {
        public WellDefinition Definition { get; private set; }
        public LabwareAddress Address { get; private set; }
        public Coordinates AbsolutePosition { get; private set; }

        public Well(LabwareAddress address, Coordinates absolutePosition, WellDefinition definition)
        {
            Address = address;
            AbsolutePosition = absolutePosition;
            Definition = definition;
        }
    }
}