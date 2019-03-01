using LHRP.Api.Instrument;
using LHRP.Api.Runtime.ErrorHandling.Errors;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public interface IErrorHandler
    {
        IErrorResolver HandleError(IInstrument instrument, RuntimeError error);         
    }
}