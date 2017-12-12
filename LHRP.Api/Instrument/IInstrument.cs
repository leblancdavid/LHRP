using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;

namespace LHRP.Api.Instrument
{
    public interface IInstrument
    {
        IPipettor GetPipettor();
        IDevice GetDevice(int id);
        ExecutionMode ExecutionMode { get; set; }
        ICommandExecutor Executor { get; }

    }
}