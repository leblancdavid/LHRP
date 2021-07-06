using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Labware.Plates;
using LHRP.Api.Labware.Tips;

namespace LHRP.TestConsole.Examples
{
    public static class ExampleLabwareCreator
    {
        public static TipRack GetTipRack()
        {
            var tipRack = new TipRack(
                new TipRackDefinition(300, 
                    "300uL Tips",
                    300.0,
                    false,
                    8, 12,
                    new Coordinates(9.0, 9.0, 9.0),
                    9.0)
            );
            return tipRack;
        }

        public static Plate GetPlate()
        {
            return new Plate(new PlateDefinition("Costar 96", new WellDefinition(), 8, 12, new Coordinates(86, 127, 14), 9.0));
        }
    }
}