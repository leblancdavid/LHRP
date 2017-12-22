using LHRP.Api.Common;
using LHRP.Api.Runtime;

namespace LHRP.Api.Devices
{
    public interface IDevice
    {
        bool IsInitialized { get; }
        Result<Process> Initialize();
        Result<Process> Deinitialize();

        IDeviceStatus DeviceStatus { get; }
    }
}