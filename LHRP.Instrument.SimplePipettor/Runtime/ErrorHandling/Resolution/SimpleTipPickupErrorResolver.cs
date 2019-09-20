using CSharpFunctionalExtensions;
using LHRP.Api.Protocol.Pipetting;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.ErrorHandling.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Instrument.SimplePipettor.Runtime.ErrorHandling
{
    public class SimpleTipPickupErrorResolver : IErrorResolver
    {
        public Result Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var tipError = error as TipPickupRuntimeError;
            if (tipError == null)
            {
                var errorMsg = $"Invalid error type {error.GetType()}";
                Console.WriteLine(errorMsg);
                return Result.Fail(errorMsg);
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
                
            }

            return Result.Ok();
        }

        private Result TryAddRetryTipPickUp(IRuntimeEngine engine, TipPickupRuntimeError error)
        {
            var tipManager = engine.Instrument.TipManager;
            var pipettor = engine.Instrument.Pipettor;

            //Make sure the tips get consumed
            for (int channel = 0; channel < pipettor.PipettorStatus.ChannelStatus.Count(); ++channel)
            {
                if (error.RequestedPattern[channel] && pipettor.PipettorStatus[channel].HasTip)
                {
                    var consumeResult = tipManager.ConsumeTip(error.RequestedPattern.GetTip(channel));
                    if (consumeResult.IsFailure)
                    {
                        return Result.Fail($"Unable to consume tips: '{consumeResult.Error}'");
                    }
                }
            }

            engine.Commands.Insert(engine.Commands.CurrentCommandIndex, 
                new PickupTips(error.ChannelErrors, 
                    error.TipTypeId,
                    engine.Commands.CurrentCommand.RetryCount + 1));

            return Result.Ok();
        }

        private Result TryPickUpNextTip(IRuntimeEngine engine, TipPickupRuntimeError error)
        {
            var tipManager = engine.Instrument.TipManager;
            var pipettor = engine.Instrument.Pipettor;

            //Make sure the tips get consumed
            for (int channel = 0; channel < pipettor.PipettorStatus.ChannelStatus.Count(); ++channel)
            {
                if (error.RequestedPattern[channel])
                {
                    var consumeResult = tipManager.ConsumeTip(error.RequestedPattern.GetTip(channel));
                    if (consumeResult.IsFailure)
                    {
                        return Result.Fail($"Unable to consume tips: '{consumeResult.Error}'");
                    }
                }
            }

            engine.Commands.Insert(engine.Commands.CurrentCommandIndex,
                new PickupTips(error.ChannelErrors,
                    error.TipTypeId));

            return Result.Ok();
        }

        private Result TryContinuePipetteSequence(IRuntimeEngine engine, TipPickupRuntimeError error)
        {

            return Result.Ok();
        }
    }
}
