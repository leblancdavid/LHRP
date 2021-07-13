using System;
using CSharpFunctionalExtensions;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;

namespace LHRP.Instrument.SimplePipettor.Runtime.ErrorHandling
{
    public class SimpleTipRackReloadRequest : IErrorResolver
    {
        public ProcessResult Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var process = new ProcessResult();
            var insuffientTipsError = error as InsuffientTipsRuntimeError;
            if(insuffientTipsError == null)
            {
                var errorMsg = $"Invalid error type {error.GetType()}";
                process.AddError(new RuntimeError(errorMsg));
                Console.WriteLine(errorMsg);
                return process;
            }

            Console.WriteLine(insuffientTipsError.Message);
            Console.Write("Would you like to reload tips (y,n): ");
            var requestInput = Console.ReadLine();
            if(requestInput != "y" && requestInput != "yes")
            {
                Console.WriteLine("Aborting the run...");
                engine.Abort();
                return process;
            }
            //Add the tip reloading logic here, logic could vary based on the instrument
            var reloadResult = engine.Instrument.TipManager.ReloadTips(insuffientTipsError.TipTypeId);
            if(reloadResult.IsFailure)
            {
                var errorMsg = $"Tip-reloading of tip type '{insuffientTipsError.TipTypeId}' failed!";
                process.AddError(new RuntimeError(errorMsg));
                Console.WriteLine();
                return process;
            }

            Console.WriteLine($"Tip-reloading of tip type '{insuffientTipsError.TipTypeId}' successful!");
            engine.Commands.MoveToLastExecutedCommand();

            return process;
        }
    }
}