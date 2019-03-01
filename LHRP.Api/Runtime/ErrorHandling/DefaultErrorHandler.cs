using LHRP.Api.Instrument;

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