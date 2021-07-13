using LHRP.Api.Runtime.ErrorHandling;

namespace LHRP.Api.Runtime.Compilation
{
    public class AutoReloadTipsCompileErrorResolver : IErrorResolver
    {
        public ProcessResult Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var process = new ProcessResult();
            var insuffientTipsError = error as InsuffientTipsRuntimeError;
            if (insuffientTipsError == null)
            {
                process.AddError(error);
                return process;
            }

            var reloadResult = engine.Instrument.TipManager.ReloadTips(insuffientTipsError.TipTypeId);
            if (reloadResult.IsFailure)
            {
                var errorMsg = $"Tip-reloading of tip type '{insuffientTipsError.TipTypeId}' failed!";
                process.AddError(new RuntimeError(errorMsg));
                return process;
            }

            engine.Commands.MoveToLastExecutedCommand();

            return process;
        }
    }
}
