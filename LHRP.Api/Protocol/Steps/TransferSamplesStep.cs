using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
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
        private ITransferOptimizer<Transfer> _transferOptimizer;
        public TransferSamplesStep(TransferSamplesStepData stepData, ITransferOptimizer<Transfer> optimizer = null)
        {
            _stepData = stepData;
            if(optimizer == null)
            {
                optimizer = new DefaultTransferOptimizer<Transfer>();
            }
            _transferOptimizer = optimizer;
        }

        public Process Run(IInstrument instrument)
        {
            var process = new Process();
            var pipettor = instrument.Pipettor;
            var tranfersResult = _stepData.Pattern.GetTransferGroups(instrument, _transferOptimizer);
            if(tranfersResult.IsFailure)
            {
                process.AddError(tranfersResult.Error);
                return process;
            }

            foreach(var transfer in tranfersResult.Value)
            {
                var tipPickupCommand = new PickupTips(transfer.ChannelPattern, _stepData.DesiredTipSize);
                var dropTips = new DropTips(_stepData.ReturnTipsToSource);
                var tipPickupResult = tipPickupCommand.Run(instrument);
                if(tipPickupResult.ContainsErrors)
                {
                    process.AppendSubProcess(dropTips.Run(instrument));
                }
                process.AppendSubProcess(tipPickupCommand.Run(instrument));

                var aspirateCommand = new Aspirate(new AspirateParameters(transfer));
                
                // var aspirateResult = pipettor.Aspirate(new AspirateCommand()
                //     {
                //         Volume = 50,
                //         Position = new Coordinates(0, 0, 0),
                //     });
                // process.AppendSubProcess(aspirateResult);

                // var dispenseResult = pipettor.Dispense(new DispenseCommand()
                //     {
                //         Volume = 50,
                //         Position = new Coordinates(0, 0, 0),
                //     });
                // process.AppendSubProcess(dispenseResult);

                //var dropTipsResult = pipettor.DropTips(new TipDropCommand()
                //    {
                //        Position = new Coordinates(0, 0, 0)
                //    });
                //process.AppendSubProcess(dropTipsResult.Value);
            }
            
            return process;
        }
    }
}