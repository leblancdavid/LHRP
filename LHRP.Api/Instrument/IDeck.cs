using LHRP.Api.Common;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Instrument
{
    public interface IDeck
    {
        Result AssignLabware(int positionId, Labware.Labware labware);
        Result<Labware.Labware> GetLabware(int positionId);
        Result<TipRack> GetTipRack(int positionId);
        
    }
}