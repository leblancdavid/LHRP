using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Labware.Tips;

namespace LHRP.TestConsole
{
    public static class TestLabwareFactory
    {
        public static TipRack GetTipRack()
        {
            var tipRack = new TipRack(
                new TipRackDefinition("300uL Tips",
                    new Dimensions(128,87,60),
                    300.0,
                    false,
                    8, 12,
                    new Coordinates(9.0, 9.0, 9.0),
                    9.0)
            );
            return tipRack;
        }
    }
}