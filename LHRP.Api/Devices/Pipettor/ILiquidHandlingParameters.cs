using System.Collections.Generic;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Devices.Pipettor
{
    public interface ILiquidHandlingParameters
    {
        double PipettingHeight { get; set; }
        PipettePositionType PipettingPositionType { get; set; }
        double MixVolume { get; set; }
        int MixCycles { get; set; }
        int MixSpeed { get; set; }
        double MixHeight { get; set; }
        PipettePositionType MixPipettingPositionType { get; set; }
        bool FollowLiquid { get; set; }
        bool EnableMad { get; set; }
    }
}