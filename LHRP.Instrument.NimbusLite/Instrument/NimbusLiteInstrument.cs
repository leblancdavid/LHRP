using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
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
            _pipettor = new IndependentChannelPipettor(Executor);

        }
        private ExecutionMode _executionMode;
        public ExecutionMode ExecutionMode 
        { 
            get
            {
                return _executionMode;
            }
            set
            {
                _executionMode = value;
                switch(_executionMode)
                {
                    case ExecutionMode.Scheduling:
                        Executor = new NimbusLiteCommandScheduler();
                        break;
                    case ExecutionMode.Execution:
                        Executor = new NimbusLiteCommandExecutor();
                        break;
                    case ExecutionMode.Simulation:
                        Executor = new NimbusLiteCommandSimulator();
                        break;
                }
                _pipettor.SetExecutor(Executor);
            }
        }

        public ICommandExecutor Executor { get; private set; }

        public IDevice GetDevice(int id)
        {
        throw new System.NotImplementedException();
        }

        private IndependentChannelPipettor _pipettor; 
        public IPipettor GetPipettor()
        {
        throw new System.NotImplementedException();
        }
    }
}