using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol
{
    public class TransferSamplesStep : IRunnable
    {
        public TransferSamplesStep()
        {

        }

        public void Run(IInstrument instrument)
        {
            var pipettor = instrument.GetPipettor();
            for(int i = 0; i < 8; ++i)
            {
                pipettor.PickupTips(new TipPickupParameters()
                {
                    ChannelPattern = "11",
                    Position = i
                });

                pipettor.Aspirate(new AspirateParameters()
                {
                    Volume = 50,
                    Position = i,
                });

                pipettor.Dispense(new DispenseParameters(){
                    Volume = 50,
                    Position = i,
                });

                pipettor.DropTips(new TipDropParameters());
            }
        }
    }
}