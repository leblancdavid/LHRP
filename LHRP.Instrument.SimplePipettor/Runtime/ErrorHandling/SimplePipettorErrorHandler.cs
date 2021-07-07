using LHRP.Api.Runtime.ErrorHandling;

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
            ConfigureResolution<TipPickupRuntimeError>(new SimpleTipPickupErrorResolver());
        }
    }
}