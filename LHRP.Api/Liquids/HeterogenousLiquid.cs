using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Liquids
{
    public class HeterogeneousLiquid : Liquid
    {
        private List<LiquidPart> _liquidParts;

        public IEnumerable<LiquidPart> LiquidParts => _liquidParts;

        public HeterogeneousLiquid()
        {
            _liquidParts = new List<LiquidPart>();
        }

        public override HeterogeneousLiquid Mix(Liquid liquid, double ratio)
        {

            return this;
        }

    }
}
