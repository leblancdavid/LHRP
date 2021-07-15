using System.Collections.Generic;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Devices.Pipettor
{
    public class AspirateParameters
    {
        public double PipettingHeight { get; set; }
        public PipettePositionType PipettingPositionType { get; set; }
        public double MixVolume { get; set; }
        public int MixCycles { get; set; }
        public int MixSpeed { get; set; }
        public double MixHeight { get; set; }
        public PipettePositionType MixPipettingPositionType { get; set; }
        public bool FollowLiquid { get; set; }
        public bool EnableMad { get; set; }

        public  AspirateParameters(double pipettingHeight = 0.0,
            PipettePositionType pipettePositionType = PipettePositionType.FromContainerBottom,
            double mixVolume = 0.0,
            int mixCycles = 0,
            double mixHeight = 0.0,
            PipettePositionType mixPipettingPositionType = PipettePositionType.FromContainerBottom,
            bool followLiquid = false,
            bool enableMad = false)
        {
            PipettingHeight = pipettingHeight;
            PipettingPositionType = pipettePositionType;
            MixVolume = mixVolume;
            MixCycles = mixCycles;
            MixHeight = mixHeight;
            MixPipettingPositionType = mixPipettingPositionType;
            FollowLiquid = followLiquid;
            EnableMad = enableMad;
        }

    }
}