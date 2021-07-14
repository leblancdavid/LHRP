using LHRP.Api.Liquids;
using System.Collections.Generic;

namespace LHRP.Api.Devices.Pipettor
{
    public interface IPipetteLogger
    {
        void Reset();
        void BeginSequence(ChannelPattern pattern);
        void LogTransfer(ChannelPattern<ChannelPipettingTransfer> transfer);
        void EndSequence(ChannelPattern pattern);
    }
}
