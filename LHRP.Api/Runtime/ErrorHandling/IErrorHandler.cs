using LHRP.Api.Instrument;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public interface IErrorHandler
    {
        IErrorResolver HandleError(IInstrument instrument, RuntimeError error);         
    }
}