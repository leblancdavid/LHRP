using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol
{
    public interface IRunnable
    {
         void Run(ICommandExecutor commandExecutor);
    }
}