using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;

namespace LHRP.Api.Instrument
{
    public class Deck : IDeck
    {
        private Dictionary<int, DeckPosition> _deckPositions = new Dictionary<int, DeckPosition>();
        public IEnumerable<DeckPosition> Positions => _deckPositions.Values.ToList();

        public Deck(List<DeckPosition> deckPositions)
        {
            foreach (var position in deckPositions)
            {
                _deckPositions[position.PositionId] = position;
            }
        }

        public DeckPosition? GetDeckPosition(int positionId)
        {
            if (!_deckPositions.ContainsKey(positionId))
            {
                return null;
            }
            return _deckPositions[positionId];
        }

        public Result AddLabware(int positionId, Labware.Labware labware)
        {
            if (!_deckPositions.ContainsKey(positionId))
            {
                return Result.Failure("Invalid deck position ID");
            }

            if(_deckPositions.Values.Any(x => x.IsOccupied && x.AssignedLabware!.InstanceId == labware.InstanceId))
            {
                return Result.Failure($"Invalid labware instance ID: Deck already contains a labware with ID {labware.InstanceId}");
            }

            return _deckPositions[positionId].Assign(labware);
        }

        public Labware.Labware? GetLabware(int instanceId)
        {
            foreach(var position in _deckPositions)
            {
                if(position.Value.IsOccupied && position.Value.AssignedLabware!.InstanceId == instanceId)
                {
                    return position.Value.AssignedLabware;
                }
            }

            return null;
        }

        public Coordinates? GetCoordinates(LabwareAddress address)
        {
            if (!_deckPositions.ContainsKey(address.InstanceId))
            {
                return null;
            }
            if (!_deckPositions[address.InstanceId].IsOccupied)
            {
                return null;
            }

            return _deckPositions[address.InstanceId].AssignedLabware!.GetRealCoordinates(address);
        }

        public IEnumerable<TipRack> GetTipRacks()
        {
            var tipRacks = new List<TipRack>();
            foreach (var position in _deckPositions.Values)
            {
                var tipRack = position.AssignedLabware as TipRack;
                if (tipRack != null)
                {
                    tipRacks.Add(tipRack);
                }
            }
            return tipRacks;
        }

        public IEnumerable<Plate> GetPlates()
        {
            var plates = new List<Plate>();
            foreach (var position in _deckPositions.Values)
            {
                var plate = position.AssignedLabware as Plate;
                if (plate != null)
                {
                    plates.Add(plate);
                }
            }
            return plates;
        }

        public IEnumerable<LiquidContainer> GetLiquidContainers()
        {
            var liquidContainers = new List<LiquidContainer>();
            foreach (var position in _deckPositions.Values)
            {
                var liquidContainer = position.AssignedLabware as LiquidContainingLabware;
                if (liquidContainer != null)
                {
                    liquidContainers.AddRange(liquidContainer.GetContainers());
                }
            }

            return liquidContainers;
        }

        public LiquidContainer? GetLiquidContainer(LabwareAddress address)
        {
            if (!_deckPositions.ContainsKey(address.InstanceId) ||
                !_deckPositions[address.InstanceId].IsOccupied)
            {
                return null;
            }

            var liquidContainerLabware = _deckPositions[address.InstanceId].AssignedLabware as LiquidContainingLabware;
            if(liquidContainerLabware == null)
            {
                return null;
            }

            return liquidContainerLabware.GetContainer(address);
        }

        public IDeck CreateSnapshot()
        {
            return new Deck(_deckPositions.Select(x => x.Value.CreateSnapshot()).ToList());
        }

        public IEnumerable<LiquidContainer> FindLiquidContainers(Liquid withLiquid)
        {
            var liquidContainers = new List<LiquidContainer>();
            foreach (var position in _deckPositions.Values)
            {
                var liquidContainer = position.AssignedLabware as LiquidContainingLabware;
                if (liquidContainer != null)
                {
                    liquidContainers.AddRange(liquidContainer.GetLiquidContainers(withLiquid));
                }
            }

            return liquidContainers;
        }
    }
}