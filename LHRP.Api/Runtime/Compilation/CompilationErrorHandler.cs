using LHRP.Api.Runtime.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Runtime.Compilation
{
    public class CompilationErrorHandler : ErrorHandler, IErrorHandler
    {
        public CompilationErrorHandler() : base()
        {
            ConfigureResolutionTable();
        }

        private void ConfigureResolutionTable()
        {
            //ConfigureResolution<InsuffientTipsRuntimeError>(new SimpleTipRackReloadRequest());
            //ConfigureResolution<TipPickupRuntimeError>(new SimpleTipPickupErrorResolver());
            //ConfigureResolution<InsufficientLiquidRuntimeError>(new SimpleLiquidRefillErrorResolver());
        }
    }
}
