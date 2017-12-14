using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol
{
    public class TransferSamplesStep : IRunnable
    {
        private IPipettor _pipettor;
        public TransferSamplesStep(IPipettor pipettor)
        {
            _pipettor = pipettor;
        }

        public void Run(ICommandExecutor commandExecutor)
        {
            for(int i = 0; i < 8; ++i)
            {
                _pipettor.PickupTips(new TipPickupParameters(), commandExecutor);
                _pipettor.Aspirate(new AspirateParameters(), commandExecutor);
                _pipettor.Dispense(new DispenseParameters(), commandExecutor);
                _pipettor.DropTips(new TipDropParameters(), commandExecutor);
            }
        }
    }
}