using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Labware;
using LHRP.Api.Runtime;

namespace LHRP.Api.Instrument
{
    public interface IDeck : IStateSnapshot<IDeck>
    {
        IEnumerable<DeckPosition> Positions { get; }
        Result AssignLabware(int positionId, Labware.Labware labware);
        DeckPosition? GetDeckPosition(int positionId);
        Labware.Labware? GetLabware(int positionId);
        IEnumerable<TipRack> GetTipRacks();
        IEnumerable<Plate> GetPlates();
        IEnumerable<LiquidContainer> GetLiquidContainers();
        LiquidContainer? GetLiquidContainer(LabwareAddress address);
        Coordinates? GetCoordinates(LabwareAddress address);
        
    }
}