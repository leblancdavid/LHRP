
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware;
using LHRP.Api.Runtime;

namespace LHRP.Api.Instrument
{
    public interface ITipManager : IStateSnapshot<ITipManager>
    {
        Result<TipChannelPattern> RequestTips(ChannelPattern pattern, int tipTypeId);
        Result ConsumeTip(Tip tip);

        Result ReloadTips(int tipTypeId);
        int GetTotalTipCount(int tipTypeId);
        double GetTipCapacity(int tipTypeId);
    }
}