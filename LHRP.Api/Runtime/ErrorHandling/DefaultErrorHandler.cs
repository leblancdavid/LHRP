using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime.ErrorHandling.Errors;
using LHRP.Api.Runtime.ErrorHandling.Resolution;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public class DefaultErrorHandler : IErrorHandler
    {
        private Dictionary<System.Type, IErrorResolver> _resolutionTable;

        public DefaultErrorHandler()
        {
            _resolutionTable = new Dictionary<System.Type, IErrorResolver>();
            _resolutionTable[typeof(InsuffientTipsRuntimeError)] = new TipReloadRequest();
        }

        public void ConfigureResolution<TErrorType>(IErrorResolver resolver) where TErrorType : RuntimeError
        {
            var errorType = typeof(TErrorType);
            _resolutionTable[errorType] = resolver;
        }

        public Result HandleError(IRuntimeEngine engine, RuntimeError error)
        {
            var errorType = error.GetType();
            if(!_resolutionTable.ContainsKey(errorType))
            {
                return Result.Fail($"No resolution found for error type '{errorType.ToString()}'");
            }

            return _resolutionTable[errorType].Resolve(engine, error);
        }
    }
}