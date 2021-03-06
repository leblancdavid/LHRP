﻿using CSharpFunctionalExtensions;
using LHRP.Api.Protocol.Pipetting;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;
using System;
using System.Linq;

namespace LHRP.Instrument.SimplePipettor.Runtime.ErrorHandling
{
    public class SimpleTipPickupErrorResolver : IErrorResolver
    {
        public ProcessResult Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var process = new ProcessResult();
            var tipError = error as TipPickupRuntimeError;
            if (tipError == null)
            {
                var errorMsg = $"Invalid error type {error.GetType()}";
                process.AddError(new RuntimeError(errorMsg));
                Console.WriteLine(errorMsg);
                return process;
            }

            Console.WriteLine(tipError.Message);
            
            Console.Write("Handling options: Retry (r), Next Tips (n), Contine (c), Abort (a): ");
            var requestInput = Console.ReadLine();
            if (requestInput == "r")
            {
                Console.WriteLine("Retrying...");
                return TryAddRetryTipPickUp(engine, tipError);
            }
            else if(requestInput == "n")
            {
                Console.WriteLine("Moving to next set of tips...");
                return TryPickUpNextTip(engine, tipError);
            }
            else if(requestInput == "c")
            {
                Console.WriteLine("Continuing...");
                return TryContinuePipetteSequence(engine, tipError);
            }
            else
            {
                Console.WriteLine("Aborting...");
                engine.Abort();
                return process;
            }

        }

        private ProcessResult TryAddRetryTipPickUp(IRuntimeEngine engine, TipPickupRuntimeError error)
        {
            var tipManager = engine.Instrument.TipManager;
            var pipettor = engine.Instrument.Pipettor;

            var process = new ProcessResult();
            //Make sure the tips get consumed
            for (int channel = 0; channel < pipettor.PipettorStatus.ChannelStatus.Count(); ++channel)
            {
                if (error.RequestedPattern.IsInUse(channel) && pipettor.PipettorStatus[channel].HasTip)
                {
                    var consumeResult = tipManager.ConsumeTip(error.RequestedPattern.GetTip(channel));
                    if (consumeResult.IsFailure)
                    {
                        process.AddError(new RuntimeError($"Unable to consume tips: '{consumeResult.Error}'"));
                        return process;
                    }
                }
            }

            engine.Commands.Insert(engine.Commands.CurrentCommandIndex, 
                new PickupTips(error.ChannelErrors, 
                    error.TipTypeId,
                    engine.Commands.CurrentCommand.RetryCount + 1));

            return process;
        }

        private ProcessResult TryPickUpNextTip(IRuntimeEngine engine, TipPickupRuntimeError error)
        {
            var tipManager = engine.Instrument.TipManager;
            var pipettor = engine.Instrument.Pipettor;

            var process = new ProcessResult();
            //Make sure the tips get consumed
            for (int channel = 0; channel < pipettor.PipettorStatus.ChannelStatus.Count(); ++channel)
            {
                if (error.RequestedPattern.IsInUse(channel))
                {
                    var consumeResult = tipManager.ConsumeTip(error.RequestedPattern.GetTip(channel));
                    if (consumeResult.IsFailure)
                    {
                        process.AddError(new RuntimeError($"Unable to consume tips: '{consumeResult.Error}'"));
                        return process;
                    }
                }
            }

            engine.Commands.Insert(engine.Commands.CurrentCommandIndex,
                new PickupTips(error.ChannelErrors,
                    error.TipTypeId));

            return process;
        }

        private ProcessResult TryContinuePipetteSequence(IRuntimeEngine engine, TipPickupRuntimeError error)
        {
            var tipManager = engine.Instrument.TipManager;
            var pipettor = engine.Instrument.Pipettor;

            var process = new ProcessResult();
            //Make sure the tips get consumed
            for (int channel = 0; channel < pipettor.PipettorStatus.ChannelStatus.Count(); ++channel)
            {
                if (error.RequestedPattern.IsInUse(channel))
                {
                    var consumeResult = tipManager.ConsumeTip(error.RequestedPattern.GetTip(channel));
                    if (consumeResult.IsFailure)
                    {
                        process.AddError(new RuntimeError($"Unable to consume tips: '{consumeResult.Error}'"));
                        return process;
                    }
                }
            }
            
            var newChannelPattern = error.RequestedPattern - error.ChannelErrors;
            int index = engine.Commands.CurrentCommandIndex;
            var nextCommand = engine.Commands.GetCommandAt(index);
            while (nextCommand != null && 
                nextCommand is IPipettingCommand &&
                !(nextCommand is PickupTips))
            {
                (nextCommand as IPipettingCommand).ApplyChannelMask(newChannelPattern);
                index++;
                nextCommand = engine.Commands.GetCommandAt(index);
            }

            return process;
        }
    }
}
