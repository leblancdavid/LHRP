using System;
using CSharpFunctionalExtensions;
using LHRP.Api.Runtime;

namespace LHRP.Api.Devices
{
    public interface IDevice
    {
        Guid DeviceId { get; }
        bool IsInitialized { get; }
        Process Initialize();
        Process Deinitialize();

        IDeviceStatus DeviceStatus { get; }
    }
}