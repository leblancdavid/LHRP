using LHRP.Api.Liquids;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public class InsufficientLiquidRuntimeError : RuntimeError
    {
        public double RemainingVolumeNeeded { get; private set; }
        public Liquid RequestedLiquid { get; private set; }

        public InsufficientLiquidRuntimeError(string errorMessage, Liquid liquid, double volume) : base(errorMessage)
        {
            RequestedLiquid = liquid;
            RemainingVolumeNeeded = volume;
        }
    }
}
