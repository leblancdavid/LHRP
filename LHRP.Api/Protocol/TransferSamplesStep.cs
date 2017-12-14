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
                _pipettor.PickupTips(new TipPickupParameters()
                {
                    ChannelPattern = "11",
                    Position = i
                }, commandExecutor);

                _pipettor.Aspirate(new AspirateParameters()
                {
                    Volume = 50,
                    Position = i,
                }, commandExecutor);

                _pipettor.Dispense(new DispenseParameters(){
                    Volume = 50,
                    Position = i,
                }, commandExecutor);
                
                _pipettor.DropTips(new TipDropParameters(), commandExecutor);
            }
        }
    }
}