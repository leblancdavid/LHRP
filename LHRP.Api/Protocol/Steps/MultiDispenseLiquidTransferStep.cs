using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Protocol.Pipetting;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.LiquidTransfers;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.Scheduling;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Protocol.Steps
{
    public class MultiDispenseLiquidTransferStep : IRunnable
    {
        private MultiDispenseLiquidTransferStepData _stepData;
        private ITransferOptimizer<LiquidToOneTransfer> _transferOptimizer;
        public MultiDispenseLiquidTransferStep(MultiDispenseLiquidTransferStepData stepData, ITransferOptimizer<LiquidToOneTransfer>? optimizer = null)
        {
            _stepData = stepData;
            if (optimizer == null)
            {
                optimizer = new DefaultLiquidToOneTransferOptimizer();
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

        public Result<Schedule> Schedule(IRuntimeEngine runtimeEngine, bool initializeResources)
        {
            var schedule = new Schedule();
            var commands = GetCommands(runtimeEngine);
            if (commands.IsFailure)
            {
                return Result.Failure<Schedule>(commands.Error);
            }

            foreach (var command in commands.Value)
            {
                var commandSchedule = command.Schedule(runtimeEngine, false);
                schedule.Combine(commandSchedule.Value);
            }
            if (initializeResources)
            {
                return runtimeEngine.Instrument.InitializeResources(schedule);
            }
            return Result.Success(schedule);
        }

        public Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            var pipettor = engine.Instrument.Pipettor;
            var tranfersResult = _stepData.Pattern.GetTransferGroups(engine.Instrument, _transferOptimizer);
            if (tranfersResult.IsFailure)
            {
                return Result.Failure<IEnumerable<IRunnableCommand>>(tranfersResult.Error);
            }

            var commands = new List<IRunnableCommand>();
            return Result.Ok<IEnumerable<IRunnableCommand>>(commands);
        }

        private Result<IEnumerable<IRunnableCommand>> GetMultiDispenseCommands(
            List<ChannelPattern<LiquidToOneTransfer>> liquidTransferGroups,
            IRuntimeEngine engine)
        {
            if (!liquidTransferGroups.Any())
            {
                return Result.Failure<IEnumerable<IRunnableCommand>>("No liquid transfer groups to process for liquid transfer");
            }

            var prePostVolume = _stepData.PreAliquotVolume + _stepData.PostAliquotVolume;
            var workingVolume = engine.Instrument.TipManager.GetTipCapacity(_stepData.TipTypeId) - prePostVolume;

            if (workingVolume < 0.0)
            {
                return Result.Failure<IEnumerable<IRunnableCommand>>("Insufficient tip capacity to handle liquid transfer with pre/post aliquots");
            }

            var multiDispenseTransferGroups = _stepData.Pattern.GetMultiDispenseTransferGroups(engine.Instrument, _transferOptimizer, workingVolume);
            if (multiDispenseTransferGroups.IsFailure)
            {
                return Result.Failure<IEnumerable<IRunnableCommand>>(multiDispenseTransferGroups.Error);
            }

            var commands = new List<IRunnableCommand>();
            if (_stepData.ReuseTips)
            {
                //commands.Add(new PickupTips(ChannelPattern.Full(liquidTransferGroups.First().ChannelPattern.NumChannels), _stepData.TipTypeId));
                foreach (var transferGroup in multiDispenseTransferGroups.Value)
                {
              
                    //commands.Add(new LiquidToOneAspirate(new AspirateParameters(), transferGroup ))
                }
            }

            return Result.Ok<IEnumerable<IRunnableCommand>>(commands);
        }
    }
}
