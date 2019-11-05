using CSharpFunctionalExtensions;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.Liquid;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling.Errors;
using LHRP.Api.Runtime.Scheduling;
using System.Collections.Generic;

namespace LHRP.Api.Protocol.Steps
{
    public class LiquidTransferStep : IRunnable
    {
        private LiquidTransferStepData _stepData;
        private ITransferOptimizer<LiquidTransfer> _transferOptimizer;
        public LiquidTransferStep(LiquidTransferStepData stepData, ITransferOptimizer<LiquidTransfer> optimizer = null)
        {
            _stepData = stepData;
            if (optimizer == null)
            {
                optimizer = new DefaultLiquidTransferOptimizer();
            }
            _transferOptimizer = optimizer;
        }


        public ProcessResult Run(IRuntimeEngine engine)
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

        public Schedule Schedule(IRuntimeEngine runtimeEngine)
        {
            var schedule = new Schedule();
            var commands = GetCommands(runtimeEngine);
            if (commands.IsFailure)
            {
                return schedule;
            }

            foreach (var command in commands.Value)
            {
                var commandSchedule = command.Schedule(runtimeEngine);
                schedule.Combine(commandSchedule);
            }

            return schedule;
        }

        public Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            //var pipettor = engine.Instrument.Pipettor;
            //var tranfersResult = _stepData.Pattern.GetTransferGroups(engine.Instrument, _transferOptimizer);
            //if (tranfersResult.IsFailure)
            //{
            //    return Result.Failure<IEnumerable<IRunnableCommand>>(tranfersResult.Error);
            //}

            var commands = new List<IRunnableCommand>();
            //foreach (var transfer in tranfersResult.Value)
            //{
            //    commands.Add(new PickupTips(transfer.ChannelPattern, _stepData.TipTypeId));
            //    commands.Add(new Aspirate(new AspirateParameters(transfer.Transfers.Select(x => x.Source).ToList(), transfer.ChannelPattern)));
            //    commands.Add(new Dispense(new DispenseParameters(transfer.Transfers.Select(x => x.Target).ToList(), transfer.ChannelPattern)));
            //    commands.Add(new DropTips(_stepData.ReturnTipsToSource));
            //}

            return Result.Ok<IEnumerable<IRunnableCommand>>(commands);
        }
    }
}
