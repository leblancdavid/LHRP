
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Instrument.TipManagement
{
    public interface ITipManager
    {
         Result<TipChannelPattern> RequestTips(ChannelPattern pattern, int tipTypeId);
         Result ConsumeTip(Tip tip);

         Result ReloadTips(int tipTypeId);
    }
}