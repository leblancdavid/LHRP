using System;
using CSharpFunctionalExtensions;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.ErrorHandling.Errors;

namespace LHRP.Instrument.SimplePipettor.Runtime.ErrorHandling
{
    public class SimpleTipRackReloadRequest : IErrorResolver
    {
        public Result Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var insuffientTipsError = error as InsuffientTipsRuntimeError;
            if(insuffientTipsError == null)
            {
                var errorMsg = $"Invalid error type {error.GetType()}";
                Console.WriteLine(errorMsg);
                return Result.Fail(errorMsg);
            }

            Console.WriteLine(insuffientTipsError.Message);
            Console.Write("Would you like to reload tips (y,n): ");
            var requestInput = Console.ReadLine();
            if(requestInput != "y" && requestInput != "yes")
            {
                Console.WriteLine("Aborting the run...");
                engine.Commands.Clear();
                return Result.Ok();
            }
            //Add the tip reloading logic here, logic could vary based on the instrument
            var reloadResult = engine.Instrument.TipManager.ReloadTips(insuffientTipsError.TipTypeId);
            if(reloadResult.IsFailure)
            {
                Console.WriteLine($"Tip-reloading of tip type '{insuffientTipsError.TipTypeId}' failed!");
                return reloadResult;
            }

            Console.WriteLine($"Tip-reloading of tip type '{insuffientTipsError.TipTypeId}' successful!");
            engine.Commands.MoveToLastExecutedCommand();

            return Result.Ok();
        }
    }
}