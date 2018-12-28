
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Instrument.TipManagement
{
    public interface ITipManager
    {
         Result<TipChannelPattern> RequestTips(ChannelPattern pattern, double tipSize);
         Result ConsumeTip(int positionId, LabwareAddress tip);
    }
}