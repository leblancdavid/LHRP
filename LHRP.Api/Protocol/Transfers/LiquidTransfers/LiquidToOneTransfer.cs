using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Protocol.Transfers.LiquidTransfers
{
    public class LiquidToOneTransfer : ITransfer
    {
        public Liquids.Liquid Source { get; private set; }
        public TransferTarget Target { get; private set; }
        public LiquidToOneTransfer(Liquids.Liquid source, TransferTarget target)
        {
            Source = source;
            Target = target;
        }
    }
}
