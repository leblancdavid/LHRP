using LHRP.Api.Runtime;

namespace LHRP.Api.Devices
{
    public interface IDevice
    {
         void SetExecutionMode(ICommandExecutor commandExecutor);
    }
}