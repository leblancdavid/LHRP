using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public class ErrorHandler : IErrorHandler
    {
        protected Dictionary<System.Type, IErrorResolver> ResolutionTable;

        public ErrorHandler()
        {
            ResolutionTable = new Dictionary<System.Type, IErrorResolver>();
            ConfigureResolution<InsuffientTipsRuntimeError>(new DefaultTipReloadRequest());
        }

        public void ConfigureResolution<TErrorType>(IErrorResolver resolver) where TErrorType : RuntimeError
        {
            var errorType = typeof(TErrorType);
            ResolutionTable[errorType] = resolver;
        }

        public Result HandleError(IRuntimeEngine engine, RuntimeError error)
        {
            var errorType = error.GetType();
            if(!ResolutionTable.ContainsKey(errorType))
            {
                return Result.Failure($"No resolution found for error type '{errorType.ToString()}'");
            }

            return ResolutionTable[errorType].Resolve(engine, error);
        }
    }
}