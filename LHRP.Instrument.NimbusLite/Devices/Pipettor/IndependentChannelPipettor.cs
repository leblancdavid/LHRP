using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Devices.Pipettor
{
    public class IndependentChannelPipettor : IPipettor
    {
        private ICommandExecutor _executor;
        public IndependentChannelPipettor(ICommandExecutor executor)
        {
            _executor = executor;
        }

        public void Aspirate(AspirateParameters parameters)
        {
            _executor.ExecuteCommand(new Command());
        }

        public void Dispense(DispenseParameters parameters)
        {
            _executor.ExecuteCommand(new Command());

        }

        public void PickupTips(TipPickupParameters parameters)
        {
            _executor.ExecuteCommand(new Command());
        }

        public void DropTips(TipDropParameters parameters)
        {
            _executor.ExecuteCommand(new Command());
        }

        public void SetExecutionMode(ICommandExecutor commandExecutor)
        {
            _executor = commandExecutor;
        }
  }
}