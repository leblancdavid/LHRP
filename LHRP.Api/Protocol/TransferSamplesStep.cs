using LHRP.Api.Devices.Pipettor;

namespace LHRP.Api.Protocol
{
    public class TransferSamplesStep : IStep
    {
        private IPipettor _pipettor;
        public TransferSamplesStep(IPipettor pipettor)
        {
            _pipettor = pipettor;
        }

        public void Run()
        {
            for(int i = 0; i < 8; ++i)
            {
                _pipettor.PickupTips(new TipPickupParameters());
                _pipettor.Aspirate(new AspirateParameters());
                _pipettor.Dispense(new DispenseParameters());
                _pipettor.DropTips(new TipDropParameters());
            }
        }
    }
}