using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;

namespace LHRP.Api.Instrument
{
    public interface IInstrument
    {
        IPipettor GetPipettor();
        IDevice GetDevice(int id);
        ICommandExecutor Executor { get; }
        ICommandScheduler Scheduler { get; }

        void Run(IRunnable run);
        Schedule Schedule(IRunnable run);
        void Simulate(IRunnable run);

    }
}