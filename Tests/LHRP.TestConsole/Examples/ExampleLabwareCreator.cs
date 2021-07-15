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
                    9.0), id
            );
            return tipRack;
        }

        public static Plate GetPlate(int id)
        {
            return new Plate(new PlateDefinition("Costar 96",
                new RectangularLabwareShape(0.0, 0.0, 0.0, new Coordinates(0.0, 0.0, 0.0)),
                new WellDefinition(new CylindricalLabwareShape(4.5, 14.5, new Coordinates())), 8, 12, new Coordinates(86, 127, 14), 9.0), id);
        }

        public static Plate GetReagentPlate1(int id)
        {
            return new Plate(new PlateDefinition("Trough 1",
                new RectangularLabwareShape(0.0, 0.0, 0.0, new Coordinates(0.0, 0.0, 0.0)), 
                new WellDefinition(new RectangularLabwareShape(100, 80, 30, new Coordinates())), 1, 1, new Coordinates(86, 127, 14), 9.0), id);
        }
    }
}