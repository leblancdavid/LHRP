using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol
{
    public class TransferSamplesStep : IStep
    {
        public TransferSamplesStep()
        {
        }

        public void Run(IInstrument instrument)
        {
            var pipettor = instrument.GetPipettor();
            for(int i = 0; i < 8; ++i)
            {
                pipettor.PickupTips(new TipPickupParameters());
                pipettor.Aspirate(new AspirateParameters());
                pipettor.Dispense(new DispenseParameters());
                pipettor.DropTips(new TipDropParameters());
            }
        }
    }
}