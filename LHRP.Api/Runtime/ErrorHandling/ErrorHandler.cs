using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public class ErrorHandler : IErrorHandler
    {
        protected Dictionary<System.Type, IErrorResolver> ResolutionTable;
        protected IErrorResolver? _defaultResolver;

        public ErrorHandler(IErrorResolver? defaultResolver = null)
        {
            ResolutionTable = new Dictionary<System.Type, IErrorResolver>();
            _defaultResolver = defaultResolver;
        }

        public void ConfigureResolution<TErrorType>(IErrorResolver resolver) where TErrorType : RuntimeError
        {
            var errorType = typeof(TErrorType);
            ResolutionTable[errorType] = resolver;
        }

        public ProcessResult HandleError(IRuntimeEngine engine, RuntimeError error)
        {
            var errorType = error.GetType();
            if (ResolutionTable.ContainsKey(errorType))
            {
                return ResolutionTable[errorType].Resolve(engine, error);
            }
            else if (_defaultResolver != null) 
            {
                return _defaultResolver.Resolve(engine, error);
            }

            var process = new ProcessResult();
            process.AddError(new RuntimeError($"No resolution found for error type '{errorType.ToString()}'"));
            return process;

        }
    }
}