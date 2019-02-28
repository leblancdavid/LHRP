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
                return Result.Fail<DeckPosition>($"Invalid deck position {positionId}");
            }
            return Result.Ok(_deckPositions[positionId]);
        }

        public Result AssignLabware(int positionId, Labware.Labware labware)
        {
            if(!_deckPositions.ContainsKey(positionId))
            {
                return Result.Fail("Invalid deck position ID");
            }

            return _deckPositions[positionId].Assign(labware);
        }

        public Result<Labware.Labware> GetLabware(int positionId)
        {
            if(!_deckPositions.ContainsKey(positionId))
            {
                return Result.Fail<Labware.Labware>("Invalid deck position ID");
            }
            if(!_deckPositions[positionId].IsOccupied)
            {
                return Result.Fail<Labware.Labware>($"No labware found in position {positionId}");
            }
            return Result.Ok(_deckPositions[positionId].AssignedLabware);
        }

        public Result<Coordinates> GetCoordinates(LabwareAddress address)
        {
             if(!_deckPositions.ContainsKey(address.PositionId))
            {
                return Result.Fail<Coordinates>("Invalid deck position ID");
            }
            if(!_deckPositions[address.PositionId].IsOccupied)
            {
               return Result.Fail<Coordinates>($"No labware found in position {address.PositionId}");
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
  }
}