using System.Collections.Generic;
using LHRP.Api.Common;
using LHRP.Api.Runtime;

namespace LHRP.Api.Devices.Pipettor
{
    public interface IPipettor : IDevice
    {
        Result<Process> Aspirate(AspirateCommand parameters);
        Result<Process> Dispense(DispenseCommand parameters);
        Result<Process> PickupTips(TipPickupParameters parameters);
        Result<Process> DropTips(TipDropCommand parameters);

        int NumberChannels { get; }
    }
}