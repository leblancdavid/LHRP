using LHRP.Api.Runtime;

namespace LHRP.Api.Devices
{
    public interface IDevice
    {
        bool IsInitialized { get; }
        ProcessResult Initialize();
        ProcessResult Deinitialize();

        IDeviceStatus DeviceStatus { get; }
    }
}