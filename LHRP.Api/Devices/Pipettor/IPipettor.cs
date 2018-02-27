using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Runtime;

namespace LHRP.Api.Devices.Pipettor
{
    public interface IPipettor : IDevice
    {
        Result<Process> Aspirate(AspirateCommand parameters);
        Result<Process> Dispense(DispenseCommand parameters);
        Result<Process> PickupTips(TipPickupParameters parameters);
        Result<Process> DropTips(TipDropCommand parameters);

        PipettorSpecification Specification { get; }
    }
}