namespace LHRP.Api.Devices.Pipettor
{
    public class DispenseParameters
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

        public DispenseParameters(double pipettingHeight = 0.0,
            PipettePositionType pipettePositionType = PipettePositionType.FromLiquidLevel,
            double mixVolume = 0.0,
            int mixCycles = 0,
            double mixHeight = 0.0,
            PipettePositionType mixPipettingPositionType = PipettePositionType.FromLiquidLevel,
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