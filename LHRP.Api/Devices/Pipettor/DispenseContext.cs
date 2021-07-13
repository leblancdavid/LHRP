namespace LHRP.Api.Devices.Pipettor
{
    public class DispenseContext
    {
        public ChannelPattern<ChannelPipettingTransfer> Targets { get; private set; }

        public DispenseContext(ChannelPattern<ChannelPipettingTransfer> targets, DispenseParameters parameters)
        {
            Targets = targets;
        }
    }
}