using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Runtime.ErrorHandling.Errors;

namespace LHRP.Api.Runtime.ErrorHandling.Resolution
{
    public class TipReloadRequest : IErrorResolver
    {
        public Result Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var insuffientTipsError = error as InsuffientTipsRuntimeError;
            if(insuffientTipsError == null)
            {
                return Result.Fail($"Invalid error type {error.GetType()}");
            }

            //Add the tip reloading logic here, logic could vary based on the instrument
            var reloadResult = engine.Instrument.TipManager.ReloadTips(insuffientTipsError.TipTypeId);
            if(reloadResult.IsFailure)
            {
                return reloadResult;
            }

            engine.Commands.MoveToLastCommand();

            return Result.Ok();
        }
    }
}