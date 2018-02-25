using System.Collections.Generic;
using LHRP.Api.Common;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Instrument.TipManagement;
using LHRP.Api.Labware;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Instrument
{
    public interface IDeck
    {
        ITipManager TipManager { get; }
        IEnumerable<DeckPosition> Positions { get; }
        Result AssignLabware(int positionId, Labware.Labware labware);
        Result<DeckPosition> GetDeckPosition(int positionId);
        Result<Labware.Labware> GetLabware(int positionId);
        Result<Coordinates> GetCoordinates(int positionId, LabwareAddress address);
        
    }
}