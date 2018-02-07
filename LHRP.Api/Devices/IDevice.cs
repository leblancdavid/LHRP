using System;
using LHRP.Api.Common;
using LHRP.Api.Runtime;

namespace LHRP.Api.Devices
{
    public interface IDevice
    {
        Guid DeviceId { get; }
        bool IsInitialized { get; }
        Result<Process> Initialize();
        Result<Process> Deinitialize();

        IDeviceStatus DeviceStatus { get; }
    }
}