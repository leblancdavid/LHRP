namespace LHRP.Api.Devices.Pipettor
{
    public class DispenseContext
    {
        public ChannelPattern<ChannelPipettingContext> Targets { get; private set; }

        public DispenseContext(ChannelPattern<ChannelPipettingContext> targets, DispenseParameters parameters)
        {
            Targets = targets;
        }
    }
}