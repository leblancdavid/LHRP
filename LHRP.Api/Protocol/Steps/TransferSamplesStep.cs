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
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.ErrorHandling.Errors;

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
                process.AddError(new RuntimeError());
                return process;
            }

            foreach(var transfer in tranfersResult.Value)
            {
                engine.Commands.Add(new PickupTips(transfer.ChannelPattern, _stepData.DesiredTipSize));
                engine.Commands.Add(new Aspirate(new AspirateParameters(transfer.Transfers.Select(x => x.Source).ToList(), 
                    transfer.ChannelPattern)));
                engine.Commands.Add(new Dispense(new DispenseParameters(transfer.Transfers.Select(x => x.Target).ToList(),
                    transfer.ChannelPattern)));
                engine.Commands.Add(new DropTips(_stepData.ReturnTipsToSource));
            }
            
            return engine.Run();
        }
    }
}