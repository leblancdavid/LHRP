using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Protocol.Transfers.LiquidTransfers
{
    public class LiquidToManyTransfer : ITransfer
    {
        public Liquids.Liquid Source { get; private set; }
        private List<TransferTarget> _targets;
        public IEnumerable<TransferTarget> Targets => _targets;
        public LiquidToManyTransfer(Liquids.Liquid source, List<TransferTarget> targets)
        {
            Source = source;
            _targets = targets;
        }

        public LiquidToManyTransfer(Liquids.Liquid source)
        {
            Source = source;
            _targets = new List<TransferTarget>();
        }

        public double GetTotalTransferVolume()
        {
            return _targets.Sum(x => x.Volume);
        }

        public void AddTransferTarget(TransferTarget transferTarget)
        {
            _targets.Add(transferTarget);
        }
    }
}
