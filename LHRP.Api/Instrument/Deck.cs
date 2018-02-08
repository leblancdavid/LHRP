using System.Collections.Generic;
using LHRP.Api.Common;
using LHRP.Api.Devices;
using LHRP.Api.Labware;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Instrument
{
    public class Deck : IDeck
    {
        private Dictionary<int, DeckPosition> _deckPositions = new Dictionary<int, DeckPosition>();

        public Deck(List<DeckPosition> deckPositions)
        {
            foreach(var position in deckPositions)
            {
                _deckPositions[position.PositionId] = position;
            }
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
                return Result<Labware.Labware>.Fail("Invalid deck position ID");
            }
            if(!_deckPositions[positionId].IsOccupied)
            {
                return Result<Labware.Labware>.Fail($"No labware found in position {positionId}");
            }
            return Result<Labware.Labware>.Ok(_deckPositions[positionId].AssignedLabware);
        }
        public Result<TipRack> GetTipRack(int positionId)
        {
            if(!_deckPositions.ContainsKey(positionId))
            {
                return Result<TipRack>.Fail("Invalid deck position ID");
            }
            if(!_deckPositions[positionId].ContainsTipRack)
            {
                return Result<TipRack>.Fail($"Position {positionId} does not contain a tip-rack");
            }
            return Result<TipRack>.Ok((TipRack)_deckPositions[positionId].AssignedLabware);
        }
        public Result<Coordinates> GetCoordinates(int positionId, LabwareAddress address)
        {
             if(!_deckPositions.ContainsKey(positionId))
            {
                return Result<Coordinates>.Fail("Invalid deck position ID");
            }
            if(!_deckPositions[positionId].IsOccupied)
            {
               return Result<Coordinates>.Fail($"No labware found in position {positionId}");
            }

            return _deckPositions[positionId].AssignedLabware.GetRealCoordinates(address);
        }
    }
}