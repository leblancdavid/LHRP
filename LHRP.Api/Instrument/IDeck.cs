using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Runtime;

namespace LHRP.Api.Instrument
{
    public interface IDeck : ISnapshotCreator<IDeck>
    {
        IEnumerable<DeckPosition> Positions { get; }
        Result AddLabware(int positionId, Labware.Labware labware);
        DeckPosition? GetDeckPosition(int positionId);
        Labware.Labware? GetLabware(int instanceId);
        IEnumerable<TipRack> GetTipRacks();
        IEnumerable<Plate> GetPlates();
        IEnumerable<LiquidContainer> GetLiquidContainers();
        IEnumerable<LiquidContainer> FindLiquidContainers(Liquid withLiquid);
        LiquidContainer? GetLiquidContainer(LabwareAddress address);
        Coordinates? GetCoordinates(LabwareAddress address);
        
    }
}