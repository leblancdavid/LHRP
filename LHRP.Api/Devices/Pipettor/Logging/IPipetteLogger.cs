using LHRP.Api.Labware;
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

        IEnumerable<ChannelPipettingTransfer> GetSourceTransfers();
        PipetteSequenceLog? GetLiquidTracking(Liquid? liquid = null, LabwareAddress? address = null);
        IEnumerable<PipetteSequenceLog> Sequences { get; }
    }
}
