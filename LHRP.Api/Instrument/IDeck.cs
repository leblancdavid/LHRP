using System.Collections.Generic;
using LHRP.Api.Common;
using LHRP.Api.Devices;
using LHRP.Api.Labware;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Instrument
{
    public interface IDeck
    {
        IEnumerable<DeckPosition> Positions { get; }
        Result AssignLabware(int positionId, Labware.Labware labware);
        Result<DeckPosition> GetDeckPosition(int positionId);
        Result<Labware.Labware> GetLabware(int positionId);
        Result<Coordinates> GetCoordinates(int positionId, LabwareAddress address);
        
    }
}