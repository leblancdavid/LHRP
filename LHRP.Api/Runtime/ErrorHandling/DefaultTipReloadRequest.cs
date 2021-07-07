using CSharpFunctionalExtensions;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public class DefaultTipReloadRequest : IErrorResolver
    {
        public Result Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var insuffientTipsError = error as InsuffientTipsRuntimeError;
            if(insuffientTipsError == null)
            {
                return Result.Failure($"Invalid error type {error.GetType()}");
            }

            //Add the tip reloading logic here, logic could vary based on the instrument
            var reloadResult = engine.Instrument.TipManager.ReloadTips(insuffientTipsError.TipTypeId);
            if(reloadResult.IsFailure)
            {
                return reloadResult;
            }

            engine.Commands.MoveToLastExecutedCommand();

            return Result.Ok();
        }
    }
}