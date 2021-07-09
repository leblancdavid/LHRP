using System.Collections.Generic;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime;

namespace LHRP.Api.Devices.Pipettor
{
    public interface IPipettor : IDevice
    {
        ProcessResult Aspirate(AspirateParameters parameters);
        ProcessResult Dispense(DispenseParameters parameters);
        ProcessResult PickupTips(TipPickupParameters parameters);
        ProcessResult DropTips(TipDropParameters parameters);

        PipettorSpecification Specification { get; }
        PipettorStatus PipettorStatus { get; }
    }
}