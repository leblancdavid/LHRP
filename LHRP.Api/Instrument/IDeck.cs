using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Labware;

namespace LHRP.Api.Instrument
{
    public interface IDeck
    {
        IEnumerable<DeckPosition> Positions { get; }
        Result AssignLabware(int positionId, Labware.Labware labware);
        DeckPosition? GetDeckPosition(int positionId);
        Labware.Labware? GetLabware(int positionId);
        IEnumerable<TipRack> GetTipRacks();
        IEnumerable<Plate> GetPlates();
        IEnumerable<LiquidContainer> GetLiquidContainers();
        Coordinates? GetCoordinates(LabwareAddress address);
        
    }
}