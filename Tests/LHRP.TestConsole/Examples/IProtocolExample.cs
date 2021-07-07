using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;

namespace LHRP.TestConsole.Examples
{
    public interface IProtocolExampleRunner
    {
        ProcessResult RunExample();
    }
}
