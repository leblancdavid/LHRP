using CSharpFunctionalExtensions;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public class DefaultTipReloadRequest : IErrorResolver
    {
        public ProcessResult Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var process = new ProcessResult();
            var insuffientTipsError = error as InsuffientTipsRuntimeError;
            if(insuffientTipsError == null)
            {
                var errorMsg = $"Invalid error type {error.GetType()}";
                process.AddError(new RuntimeError(errorMsg));
                return process;
            }

            //Add the tip reloading logic here, logic could vary based on the instrument
            var reloadResult = engine.Instrument.TipManager.ReloadTips(insuffientTipsError.TipTypeId);
            if(reloadResult.IsFailure)
            {
                process.AddError(new RuntimeError(reloadResult.Error));
                return process;
            }

            engine.Commands.MoveToLastExecutedCommand();

            return process;
        }
    }
}