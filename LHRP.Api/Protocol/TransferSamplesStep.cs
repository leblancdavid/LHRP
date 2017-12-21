using LHRP.Api.Devices;
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
                        Position = new Position()
                    }));

                result.AppendSubProcessResult(
                    pipettor.Aspirate(new AspirateParameters()
                    {
                        Volume = 50,
                        Position = new Position(),
                    }));

                result.AppendSubProcessResult(
                    pipettor.Dispense(new DispenseParameters()
                    {
                        Volume = 50,
                        Position = new Position(),
                    }));

                result.AppendSubProcessResult(
                    pipettor.DropTips(new TipDropParameters()
                    {
                        Position = new Position()
                    }));
            }

            return result;
        }
    }
}