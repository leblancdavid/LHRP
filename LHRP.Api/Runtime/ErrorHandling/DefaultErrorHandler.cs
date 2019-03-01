using LHRP.Api.Instrument;
using LHRP.Api.Runtime.ErrorHandling.Errors;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public class DefaultErrorHandler : IErrorHandler
    {
        public IErrorResolver HandleError(IInstrument instrument, RuntimeError error)
        {
            throw new System.NotImplementedException();
        }
    }
}