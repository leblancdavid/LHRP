using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime.ErrorHandling.Errors;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public interface IErrorHandler
    {
        Result HandleError(IRuntimeEngine engine, RuntimeError error);
        void ConfigureResolution<TErrorType>(IErrorResolver resolver) where TErrorType : RuntimeError;
    }
}