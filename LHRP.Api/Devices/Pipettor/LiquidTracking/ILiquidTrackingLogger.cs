namespace LHRP.Api.Devices.Pipettor
{
    public interface ILiquidTrackingLogger
    {
        void Reset();
        void LogTransfer(ChannelPipettingTransfer transfer);
    }
}
