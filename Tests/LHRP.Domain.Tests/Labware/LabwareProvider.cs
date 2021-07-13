using LHRP.Api.CoordinateSystem;
using LHRP.Api.Labware;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Domain.Tests.Labware
{
    public static class LabwareProvider
    {
        public static TipRack Get300TipRack()
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

        public static TipRack Get50TipRack()
        {
            var tipRack = new TipRack(
                new TipRackDefinition(50,
                    "50uL Tips",
                    50.0,
                    false,
                    8, 12,
                    new Coordinates(9.0, 9.0, 9.0),
                    9.0)
            );
            return tipRack;
        }

        public static Plate Get96WellPlate()
        {
            return new Plate(new PlateDefinition("Plate 96", new WellDefinition(250), 8, 12, new Coordinates(86, 127, 14), 9.0));
        }

        public static Plate GetSingleTroughReagentPlate()
        {
            return new Plate(new PlateDefinition("Trough 1", new WellDefinition(5000), 1, 1, new Coordinates(86, 127, 14), 9.0));
        }
    }
}
