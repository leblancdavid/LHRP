using LHRP.Api.Runtime;

namespace LHRP.Api.Devices.Pipettor
{
    public class AspirateContext
    {
        public ChannelPattern<ChannelPipettingTransfer> Targets { get; private set; }

        public AspirateContext(ChannelPattern<ChannelPipettingTransfer> targets, AspirateParameters parameters)
        {
            Targets = targets;
        }

        ProcessResult Validate(IPipettor pipettor)
        {
            return new ProcessResult();
        }

    }
}
