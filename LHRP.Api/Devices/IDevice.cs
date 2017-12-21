using LHRP.Api.Runtime;

namespace LHRP.Api.Devices
{
    public interface IDevice
    {
        bool IsInitialized { get; }
        void Initialize();
        void Deinitialize();

        IDeviceStatus DeviceStatus { get; }
    }
}