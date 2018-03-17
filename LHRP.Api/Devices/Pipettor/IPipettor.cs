using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Runtime;

namespace LHRP.Api.Devices.Pipettor
{
    public interface IPipettor : IDevice
    {
        Process Aspirate(AspirateCommand parameters);
        Process Dispense(DispenseCommand parameters);
        Process PickupTips(TipPickupParameters parameters);
        Process DropTips(TipDropParameters parameters);

        PipettorSpecification Specification { get; }
    }
}