﻿
using LHRP.Api.Instrument;
using LHRP.Api.Labware;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Domain.Tests.Labware
{
    public static class LabwareProvider
    {
        public static TipRack Get300TipRack(int id)
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

        public static TipRack Get50TipRack(int id)
        {
            var tipRack = new TipRack(
                new TipRackDefinition(50,
                    "50uL Tips",
                    50.0,
                    false,
                    8, 12,
                    new Coordinates(9.0, 9.0, 9.0),
                    9.0), id
            );
            return tipRack;
        }

        public static Plate Get96WellPlate(int id)
        {
            return new Plate(new PlateDefinition("Plate 96",
                new RectangularLabwareShape(0.0, 0.0, 0.0, new Coordinates(0.0, 0.0, 0.0)), 
                new WellDefinition(new CylindricalLabwareShape(4.5, 14.5, new Coordinates())), 8, 12, new Coordinates(86, 127, 14), 9.0), id);
        }

        public static Plate GetSingleTroughReagentPlate(int id)
        {
            return new Plate(new PlateDefinition("Trough 1",
                new RectangularLabwareShape(0.0, 0.0, 0.0, new Coordinates(0.0, 0.0, 0.0)), 
                new WellDefinition(new CylindricalLabwareShape(4.5, 14.5, new Coordinates())), 1, 1, new Coordinates(86, 127, 14), 9.0), id);
        }
    }
}
