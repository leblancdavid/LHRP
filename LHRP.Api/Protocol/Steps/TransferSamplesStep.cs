using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol.Pipetting;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.OneToOne;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol.Steps
{
    public class TransferSamplesStep : IRunnable
    {
        private TransferSamplesStepData _stepData;
        private ITransferOptimizer<OneToOneTransfer> _transferOptimizer;
        public TransferSamplesStep(TransferSamplesStepData stepData, ITransferOptimizer<OneToOneTransfer> optimizer = null)
        {
            _stepData = stepData;
            if(optimizer == null)
            {
                optimizer = new DefaultOneToOneTransferOptimizer();
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

                var aspirateCommand = new Aspirate(new AspirateParameters(transfer.Transfers.Select(x => x.Source).ToList(), 
                    transfer.ChannelPattern));
                process.AppendSubProcess(aspirateCommand.Run(instrument));

                var dispenseCommand = new Dispense(new DispenseParameters(transfer.Transfers.Select(x => x.Target).ToList(),
                    transfer.ChannelPattern));
                process.AppendSubProcess(dispenseCommand.Run(instrument));
            }
            
            return process;
        }
    }
}