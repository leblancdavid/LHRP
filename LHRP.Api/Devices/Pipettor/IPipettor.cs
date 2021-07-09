using System.Collections.Generic;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime;

namespace LHRP.Api.Devices.Pipettor
{
    public interface IPipettor : IDevice
    {
        ProcessResult Aspirate(AspirateContext parameters,
            ChannelPattern<TransferTarget> targets);
        ProcessResult Dispense(DispenseContext parameters,
            ChannelPattern<TransferTarget> targets);
        ProcessResult PickupTips(TipPickupParameters parameters);
        ProcessResult DropTips(TipDropParameters parameters);

        PipettorSpecification Specification { get; }
        PipettorStatus PipettorStatus { get; }
    }
}