using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Protocol.Pipetting;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.LiquidTransfers;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling.Errors;
using LHRP.Api.Runtime.Scheduling;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Protocol.Steps
{
    public class LiquidTransferStep : IRunnable
    {
        private LiquidTransferStepData _stepData;
        private ITransferOptimizer<LiquidToOneTransfer> _transferOptimizer;
        public LiquidTransferStep(LiquidTransferStepData stepData, ITransferOptimizer<LiquidToOneTransfer> optimizer = null)
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
            var pipettor = engine.Instrument.Pipettor;
            var tranfersResult = _stepData.Pattern.GetTransferGroups(engine.Instrument, _transferOptimizer);
            if (tranfersResult.IsFailure)
            {
                return Result.Failure<IEnumerable<IRunnableCommand>>(tranfersResult.Error);
            }
            
            var commands = new List<IRunnableCommand>();
            return Result.Ok<IEnumerable<IRunnableCommand>>(commands);
        }

        private Result<IEnumerable<IRunnableCommand>> GetMultiDispenseCommandsWithTipReuse(
            List<TransferGroup<ITransfer>> liquidTransferGroups,
            IRuntimeEngine engine)
        {
            if(!liquidTransferGroups.Any())
            {
                return Result.Failure<IEnumerable<IRunnableCommand>>("No liquid transfer groups to process for liquid transfer");
            }
            
            var prePostVolume = _stepData.PreAliquotVolume + _stepData.PostAliquotVolume;
            var workingVolume = engine.Instrument.TipManager.GetTipCapacity(_stepData.TipTypeId) - prePostVolume;

            if(workingVolume < 0.0)
            {
                return Result.Failure<IEnumerable<IRunnableCommand>>("Insufficient tip capacity to handle liquid transfer with pre/post aliquots");
            }
            
            var channelPattern = GetOverallChannelPattern(liquidTransferGroups);
            int numChannels = channelPattern.NumChannels;

            var commands = new List<IRunnableCommand>();
            commands.Add(new PickupTips(channelPattern, _stepData.TipTypeId));
            for (int i = 0; i < liquidTransferGroups.Count; ++i)
            {
                var liquidTargets = new List<LiquidTarget>();
                int numTransfers = 0;
                for(int j = i; j < liquidTransferGroups.Count; ++j)
                {
                    for(int channel = 0; channel < numChannels; ++channel)
                    {
                        //if(liquidTransferGroups[j].ChannelPattern[channel] &&)
                    }
                }
            }
            commands.Add(new DropTips(_stepData.ReturnTipsToSource));

            //foreach (var transfer in liquidTransferGroups)
            //{
            //    commands.Add(new PickupTips(transfer.ChannelPattern, _stepData.TipTypeId));
            //    commands.Add(new LiquidTargetAspirate(new AspirateParameters(), transfer.Transfers.Select(x => )) .ToList(), transfer.ChannelPattern)));
            //    commands.Add(new Dispense(new DispenseParameters(transfer.Transfers.Select(x => x.Target).ToList(), transfer.ChannelPattern)));
            //    commands.Add(new DropTips(_stepData.ReturnTipsToSource));
            //}

            return Result.Ok<IEnumerable<IRunnableCommand>>(commands);
        }

        private ChannelPattern GetOverallChannelPattern(List<TransferGroup<ITransfer>> liquidTransferGroups)
        {
            if(!liquidTransferGroups.Any())
            {
                return ChannelPattern.Empty(0);
            }

            var overallChannelPattern = ChannelPattern.Empty(liquidTransferGroups.First().ChannelPattern.NumChannels);
            foreach(var transfer in liquidTransferGroups)
            {
                overallChannelPattern = overallChannelPattern + transfer.ChannelPattern;
            }

            return overallChannelPattern;
        }
    }
}
