using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Instrument.TipManagement;
using LHRP.Api.Labware;
using LHRP.Api.Labware.Plates;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Instrument
{
    public class Deck : IDeck
    {
        private Dictionary<int, DeckPosition> _deckPositions = new Dictionary<int, DeckPosition>();
        public IEnumerable<DeckPosition> Positions => _deckPositions.Values.ToList();

        public Deck(List<DeckPosition> deckPositions)
        {
            foreach(var position in deckPositions)
            {
                _deckPositions[position.PositionId] = position;
            }
        }

        public Result<DeckPosition> GetDeckPosition(int positionId)
        {
            if(!_deckPositions.ContainsKey(positionId))
            {
                return Result.Failure<DeckPosition>($"Invalid deck position {positionId}");
            }
            return Result.Ok(_deckPositions[positionId]);
        }

        public Result AssignLabware(int positionId, Labware.Labware labware)
        {
            if(!_deckPositions.ContainsKey(positionId))
            {
                return Result.Failure("Invalid deck position ID");
            }

            return _deckPositions[positionId].Assign(labware);
        }

        public Result<Labware.Labware> GetLabware(int positionId)
        {
            if(!_deckPositions.ContainsKey(positionId))
            {
                return Result.Failure<Labware.Labware>("Invalid deck position ID");
            }
            if(!_deckPositions[positionId].IsOccupied)
            {
                return Result.Failure<Labware.Labware>($"No labware found in position {positionId}");
            }
            return Result.Ok(_deckPositions[positionId].AssignedLabware);
        }

        public Result<Coordinates> GetCoordinates(LabwareAddress address)
        {
             if(!_deckPositions.ContainsKey(address.PositionId))
            {
                return Result.Failure<Coordinates>("Invalid deck position ID");
            }
            if(!_deckPositions[address.PositionId].IsOccupied)
            {
               return Result.Failure<Coordinates>($"No labware found in position {address.PositionId}");
            }

            return _deckPositions[address.PositionId].AssignedLabware.GetRealCoordinates(address);
        }

        public IEnumerable<TipRack> GetTipRacks()
        {
            var tipRacks = new List<TipRack>();
            foreach(var position in _deckPositions.Values)
            {
                if(position.IsOccupied && position.AssignedLabware is TipRack)
                {
                    tipRacks.Add(position.AssignedLabware as TipRack);
                }
            }
            return tipRacks;
        }

        public IEnumerable<Plate> GetPlates()
        {
            var plates = new List<Plate>();
            foreach(var position in _deckPositions.Values)
            {
                if(position.IsOccupied && position.AssignedLabware is Plate)
                {
                    plates.Add(position.AssignedLabware as Plate);
                }
            }
            return plates;
        }

        public IEnumerable<LiquidContainer> GetLiquidContainers()
        {
            var liquidContainers = new List<LiquidContainer>();
            foreach (var position in _deckPositions.Values)
            {
                if (position.IsOccupied && position.AssignedLabware is LiquidContainingLabware)
                {
                    var containerLabware = position.AssignedLabware as LiquidContainingLabware;
                    liquidContainers.AddRange(containerLabware.GetContainers());
                }
            }

            return liquidContainers;
        }
    }
}