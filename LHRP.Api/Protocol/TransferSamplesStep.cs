using LHRP.Api.Common;
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

        public Result<Process> Run(IInstrument instrument)
        {
            var process = new Process();
            var pipettor = instrument.GetPipettor();
            for(int i = 0; i < 8; ++i)
            {
                var tipPickupResult = pipettor.PickupTips(new TipPickupCommand()
                    {
                        ChannelPattern = "1",
                        Position = new Position()
                    });
                process.AppendSubProcess(tipPickupResult.Value);
                
                var aspirateResult = pipettor.Aspirate(new AspirateCommand()
                    {
                        Volume = 50,
                        Position = new Position(),
                    });
                process.AppendSubProcess(aspirateResult.Value);

                var dispenseResult = pipettor.Dispense(new DispenseCommand()
                    {
                        Volume = 50,
                        Position = new Position(),
                    });
                process.AppendSubProcess(dispenseResult.Value);

                var dropTipsResult = pipettor.DropTips(new TipDropCommand()
                    {
                        Position = new Position()
                    });
                process.AppendSubProcess(dropTipsResult.Value);
            }

            return Result<Process>.Ok(process);
        }
    }
}