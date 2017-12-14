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
            commandExecutor.ExecuteCommand(new AspirateCommand(parameters.Volume, parameters.Position));
        }

        public void Dispense(DispenseParameters parameters, ICommandExecutor commandExecutor)
        {
            commandExecutor.ExecuteCommand(new DispenseCommand(parameters.Volume, parameters.Position));

        }

        public void PickupTips(TipPickupParameters parameters, ICommandExecutor commandExecutor)
        {
            commandExecutor.ExecuteCommand(new PickupTipsCommand(parameters.ChannelPattern, parameters.Position));
        }

        public void DropTips(TipDropParameters parameters, ICommandExecutor commandExecutor)
        {
            commandExecutor.ExecuteCommand(new DropTipsCommand());
        }
  }
}