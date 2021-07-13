using System;

namespace LHRP.Api.Devices.Pipettor
{
    public class InMemoryLiquidTrackingLogger : ILiquidTrackingLogger
    {
        public void LogTransfer(ChannelPipettingTransfer transfer)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
