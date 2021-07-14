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
            return new Plate(new PlateDefinition("Costar 96", new WellDefinition(250), 8, 12, new Coordinates(86, 127, 14), 9.0), id);
        }

        public static Plate GetReagentPlate1(int id)
        {
            return new Plate(new PlateDefinition("Trough 1", new WellDefinition(5000), 1, 1, new Coordinates(86, 127, 14), 9.0), id);
        }
    }
}