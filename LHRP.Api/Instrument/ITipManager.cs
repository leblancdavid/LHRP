using LHRP.Api.Common;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Instrument
{
    public interface ITipManager
    {
         Result AssignTipRack(int positionId, TipRack tipRack);
         Result RequestTips(ChannelPattern pattern);
    }
}