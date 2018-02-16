using LHRP.Api.Common;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol
{
    public class TransferSamplesStep : IRunnable
    {
        private TransferPattern _pattern;
        public TransferSamplesStep(TransferPattern pattern)
        {
            _pattern = pattern;
        }

        public Result<Process> Run(IInstrument instrument)
        {
            var process = new Process();
            var pipettor = instrument.GetPipettor();
            var tranfers = _pattern.GetTransferGroups(instrument);
            foreach(var t in tranfers)
            {
                var tipPickupResult = pipettor.PickupTips(new TipPickupCommand()
                    {
                        ChannelPattern = t.ChannelPattern,
                        Position = new Coordinates(0, 0, 0)
                    });
                process.AppendSubProcess(tipPickupResult.Value);
                
                var aspirateResult = pipettor.Aspirate(new AspirateCommand()
                    {
                        Volume = 50,
                        Position = new Coordinates(0, 0, 0),
                    });
                process.AppendSubProcess(aspirateResult.Value);

                var dispenseResult = pipettor.Dispense(new DispenseCommand()
                    {
                        Volume = 50,
                        Position = new Coordinates(0, 0, 0),
                    });
                process.AppendSubProcess(dispenseResult.Value);

                var dropTipsResult = pipettor.DropTips(new TipDropCommand()
                    {
                        Position = new Coordinates(0, 0, 0)
                    });
                process.AppendSubProcess(dropTipsResult.Value);
            }
            
            return Result<Process>.Ok(process);
        }
    }
}