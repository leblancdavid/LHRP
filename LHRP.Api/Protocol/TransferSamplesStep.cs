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

        public ProcessResult Run(IInstrument instrument)
        {
            var result = new ProcessResult();
            var pipettor = instrument.GetPipettor();
            for(int i = 0; i < 8; ++i)
            {
                result.AppendSubProcessResult(
                    pipettor.PickupTips(new TipPickupParameters()
                    {
                        ChannelPattern = "11",
                        Position = i
                    }));

                result.AppendSubProcessResult(
                    pipettor.Aspirate(new AspirateParameters()
                    {
                        Volume = 50,
                        Position = i,
                    }));

                result.AppendSubProcessResult(
                    pipettor.Dispense(new DispenseParameters()
                    {
                        Volume = 50,
                        Position = i,
                    }));

                result.AppendSubProcessResult(pipettor.DropTips(new TipDropParameters()));
            }

            return result;
        }
    }
}