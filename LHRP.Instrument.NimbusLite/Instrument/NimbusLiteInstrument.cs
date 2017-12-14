using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;
using LHRP.Instrument.NimbusLite.Devices.Pipettor;
using LHRP.Instrument.NimbusLite.Runtime;

namespace LHRP.Instrument.NimbusLite.Instrument
{
    public class NimbusLiteInstrument : IInstrument
    {
        public NimbusLiteInstrument()
        {
            Executor = new NimbusLiteCommandExecutor();
            _pipettor = new IndependentChannelPipettor();

        }
        
        public ICommandExecutor Executor { get; private set; }

        public ICommandScheduler Scheduler { get; private set; }

        public IDevice GetDevice(int id)
        {
            throw new System.NotImplementedException();
        }

        private IndependentChannelPipettor _pipettor; 
        public IPipettor GetPipettor()
        {
            return _pipettor;
        }

        public void Run(IRunnable run)
        {
            run.Run(Executor);
        }

        public Schedule Schedule(IRunnable run)
        {
            run.Run(Scheduler);
            return Scheduler.GetSchedule();
        }

        public void Simulate(IRunnable run)
        {
            run.Run(new NimbusLiteCommandSimulator());
        }
  }
}