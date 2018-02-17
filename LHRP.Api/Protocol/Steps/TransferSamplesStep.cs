using LHRP.Api.Common;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol.Pipetting;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol.Steps
{
    public class TransferSamplesStep : IRunnable
    {
        private TransferSamplesStepData _stepData;
        public TransferSamplesStep(TransferSamplesStepData stepData)
        {
            _stepData = stepData;
        }

        public Result<Process> Run(IInstrument instrument)
        {
            var process = new Process();
            var pipettor = instrument.GetPipettor();
            var tranfers = _stepData.Pattern.GetTransferGroups(instrument);
            foreach(var t in tranfers)
            {
                var tipPickupCommand = new PickupTips(new PickupTipsOptions(t.ChannelPattern, _stepData.DesiredTipSize));
                process.AppendSubProcess(tipPickupCommand.Run(instrument).Value);
                
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