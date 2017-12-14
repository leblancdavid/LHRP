using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Devices.Pipettor
{
    public class IndependentChannelPipettor : IPipettor
    {
        public IndependentChannelPipettor()
        {
        }

        public void Aspirate(AspirateParameters parameters, ICommandExecutor commandExecutor)
        {
            commandExecutor.ExecuteCommand(new Command());
        }

        public void Dispense(DispenseParameters parameters, ICommandExecutor commandExecutor)
        {
            commandExecutor.ExecuteCommand(new Command());

        }

        public void PickupTips(TipPickupParameters parameters, ICommandExecutor commandExecutor)
        {
            commandExecutor.ExecuteCommand(new Command());
        }

        public void DropTips(TipDropParameters parameters, ICommandExecutor commandExecutor)
        {
            commandExecutor.ExecuteCommand(new Command());
        }
  }
}