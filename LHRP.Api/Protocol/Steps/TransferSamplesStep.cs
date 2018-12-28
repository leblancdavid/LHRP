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

        public Process Run(IRuntimeEngine engine)
        {
            var process = new Process();
            var pipettor = engine.Instrument.Pipettor;
            var tranfersResult = _stepData.Pattern.GetTransferGroups(engine.Instrument, _transferOptimizer);
            if(tranfersResult.IsFailure)
            {
                process.AddError(tranfersResult.Error);
                return process;
            }

            foreach(var transfer in tranfersResult.Value)
            {
                var tipPickupCommand = new PickupTips(transfer.ChannelPattern, _stepData.DesiredTipSize);
                var dropTips = new DropTips(_stepData.ReturnTipsToSource);
                var tipPickupResult = tipPickupCommand.Run(engine);
                if(tipPickupResult.ContainsErrors)
                {
                    process.AppendSubProcess(dropTips.Run(engine));
                }
                process.AppendSubProcess(tipPickupCommand.Run(engine));

                var aspirateCommand = new Aspirate(new AspirateParameters(transfer.Transfers.Select(x => x.Source).ToList(), 
                    transfer.ChannelPattern));
                process.AppendSubProcess(aspirateCommand.Run(engine));

                var dispenseCommand = new Dispense(new DispenseParameters(transfer.Transfers.Select(x => x.Target).ToList(),
                    transfer.ChannelPattern));
                process.AppendSubProcess(dispenseCommand.Run(engine));
            }
            
            return process;
        }
    }
}