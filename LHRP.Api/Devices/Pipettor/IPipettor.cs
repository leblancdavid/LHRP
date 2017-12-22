using System.Collections.Generic;
using LHRP.Api.Common;
using LHRP.Api.Runtime;

namespace LHRP.Api.Devices.Pipettor
{
    public interface IPipettor : IDevice
    {
        Result<Process> Aspirate(AspirateParameters parameters);
        Result<Process> Dispense(DispenseParameters parameters);
        Result<Process> PickupTips(TipPickupParameters parameters);
        Result<Process> DropTips(TipDropParameters parameters);

        int NumberChannels { get; }
    }
}