using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Protocol.Pipetting;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.LiquidTransfers;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Protocol.Steps
{
    public class LiquidTransferStep : BaseRunnable, IRunnable
    {
        private LiquidTransferStepData _stepData;
        private ITransferOptimizer<LiquidToOneTransfer> _transferOptimizer;
        public LiquidTransferStep(LiquidTransferStepData stepData, ITransferOptimizer<LiquidToOneTransfer>? optimizer = null)
        {
            _stepData = stepData;
            if (optimizer == null)
            {
                optimizer = new DefaultLiquidToOneTransferOptimizer();
            }
            _transferOptimizer = optimizer;
        }


        public override ProcessResult Run(IRuntimeEngine engine)
        {
            var process = new ProcessResult();

            var commands = GetCommands(engine);
            if (commands.IsFailure)
            {
                process.AddError(new RuntimeError(commands.Error));
                return process;
            }

            foreach (var command in commands.Value)
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
            if(_stepData.ReuseTips)
            {
                commands.Add(new PickupTips(ChannelPattern.Full(tranfersResult.Value.First().NumChannels), _stepData.TipTypeId));
                foreach(var transferGroup in tranfersResult.Value)
                {
                    commands.Add(new LiquidToOneAspirate(new AspirateParameters(), transferGroup));
                    commands.Add(new Dispense(new DispenseParameters(), transferGroup.ToTargetTransfer()));
                }
                commands.Add(new DropTips(_stepData.ReturnTipsToSource));
            }
            else
            {
                foreach (var transferGroup in tranfersResult.Value)
                {
                    commands.Add(new PickupTips(transferGroup, _stepData.TipTypeId));
                    commands.Add(new LiquidToOneAspirate(new AspirateParameters(), transferGroup));
                    commands.Add(new Dispense(new DispenseParameters(), transferGroup.ToTargetTransfer()));
                    commands.Add(new DropTips(_stepData.ReturnTipsToSource));
                }
            }
            return Result.Ok<IEnumerable<IRunnableCommand>>(commands);
        }
    }
}
