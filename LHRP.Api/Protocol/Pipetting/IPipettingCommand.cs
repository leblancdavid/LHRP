using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol.Pipetting
{
    public interface IPipettingCommand : IRunnableCommand
    {
        void ApplyChannelMask(ChannelPattern<bool> channelPattern);
    }
}
