using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.ErrorHandling.Errors;
using LHRP.Instrument.SimplePipettor.Runtime.ErrorHandling.Resolution;

namespace LHRP.Instrument.SimplePipettor.Runtime.ErrorHandling
{
    public class SimplePipettorErrorHandler : ErrorHandler, IErrorHandler
    {
        public SimplePipettorErrorHandler() :base()
        {
            ConfigureResolutionTable();
        }

        private void ConfigureResolutionTable()
        {
            ConfigureResolution<InsuffientTipsRuntimeError>(new SimpleTipRackReloadRequest());
        }
    }
}