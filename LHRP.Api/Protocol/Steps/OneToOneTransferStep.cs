using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Protocol.Pipetting;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.OneToOne;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;

namespace LHRP.Api.Protocol.Steps
{
    public class OneToOneTransferStep : BaseRunnable, IRunnable
    {
        private OneToOneTransferStepData _stepData;
        private ITransferOptimizer<OneToOneTransfer> _transferOptimizer;

        public OneToOneTransferStep(OneToOneTransferStepData stepData, ITransferOptimizer<OneToOneTransfer>? optimizer = null)
        {
            _stepData = stepData;
            if(optimizer == null)
            {
                optimizer = new DefaultOneToOneTransferOptimizer();
            }
            _transferOptimizer = optimizer;
        }

        public override ProcessResult Run(IRuntimeEngine engine)
        {
            var process = new ProcessResult();

            var commands = GetCommands(engine);
            if(commands.IsFailure)
            {
                process.AddError(new RuntimeError(commands.Error));
                return process;
            }

            foreach(var command in commands.Value)
            {
                engine.Commands.Add(command);
            }
            
            return engine.Run();
        }

        public override Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            var pipettor = engine.Instrument.Pipettor;
            var tranfersResult = _stepData.Pattern.GetTransferGroups(engine.Instrument, _transferOptimizer);
            if (tranfersResult.IsFailure)
            {
                return Result.Failure<IEnumerable<IRunnableCommand>>(tranfersResult.Error);
            }

            var commands = new List<IRunnableCommand>();
            foreach (var transfer in tranfersResult.Value)
            {
                commands.Add(new PickupTips(transfer, _stepData.TipTypeId));
                commands.Add(new TransferTargetAspirate(_stepData.AspirateParameters, transfer.ToSourceTransfer()));
                commands.Add(new Dispense(_stepData.DispenseParameters, transfer.ToTargetTransfer()));
                commands.Add(new DropTips(_stepData.ReturnTipsToSource));
            }

            return Result.Ok<IEnumerable<IRunnableCommand>>(commands);
        }
    }
}