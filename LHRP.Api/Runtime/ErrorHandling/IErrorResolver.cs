using CSharpFunctionalExtensions;
using LHRP.Api.Runtime.ErrorHandling.Errors;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public interface IErrorResolver 
    {
         Result Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError;
    }
}