using CSharpFunctionalExtensions;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public interface IErrorResolver 
    {
        ProcessResult Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError;
    }
}