using LHRP.Api.Instrument;
using LHRP.Api.Labware;

namespace LHRP.TestConsole.Examples
{
    public static class ExampleLabwareCreator
    {
        public static TipRack GetTipRack(int id)
        {
            var tipRack = new TipRack(
                new TipRackDefinition(300, 
                    "300uL Tips",
                    300.0,
                    false,
                    8, 12,
                    new Coordinates(9.0, 9.0, 9.0),
                    9.0,
                    new RectangularLabwareShape(127.76, 85.48, 55.0)), id
            );
            return tipRack;
        }

        public static Plate GetPlate(int id)
        {
            return new Plate(new PlateDefinition("Plate 96",
                new RectangularLabwareShape(127.76, 85.48, 16.0),
                new WellDefinition(new CylindricalLabwareShape(4.5, 14)),
                8, 12,
                new Coordinates(9.88, 6.74, 2.0), 9.0), id);
        }

        public static Plate GetReagentPlate1(int id)
        {
            return new Plate(new PlateDefinition("Trough 1",
                new RectangularLabwareShape(127.76, 85.48, 32.0),
                new WellDefinition(new RectangularLabwareShape(108, -72, 30)), 1, 1, new Coordinates(9.88, 6.74, 2.0), 9.0), id);
        }
    }
}