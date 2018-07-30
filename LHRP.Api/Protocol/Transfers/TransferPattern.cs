using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol.Transfers
{
    public class TransferPattern
    {
        private List<Transfer> _transfers = new List<Transfer>();
        public IEnumerable<Transfer> Transfers => _transfers;

        public TransferPattern()
        {
            _transfers = new List<Transfer>();
        }

        public TransferPattern(List<Transfer> transfers)
        {
            _transfers = transfers;
        }

        public void AddTransfer(Transfer tranfer)
        {
            _transfers.Add(tranfer);
        }
        
        public Result<IEnumerable<TransferGroup>> GetTransferGroups(IInstrument instrument, ITransferOptimizer optimizer = null)
        {
            if(optimizer == null)
            {
                optimizer = new DefaultTransferOptimizer();
            }
            var transferGroups = optimizer.OptimizeTransfers(_transfers, instrument);
            return transferGroups;
        }

    }
}