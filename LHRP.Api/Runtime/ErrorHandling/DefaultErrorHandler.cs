using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime.ErrorHandling.Errors;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public class DefaultErrorHandler : IErrorHandler
    {
        private Dictionary<System.Type, IErrorResolver> _resolutionTable = new Dictionary<System.Type, IErrorResolver>();

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