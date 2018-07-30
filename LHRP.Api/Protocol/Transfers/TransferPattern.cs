using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol.Transfers
{
    public class TransferPattern<T> where T : Transfer
    {
        private List<T> _transfers = new List<T>();
        public IEnumerable<T> Transfers => _transfers;

        public TransferPattern()
        {
            _transfers = new List<T>();
        }

        public TransferPattern(List<T> transfers)
        {
            _transfers = transfers;
        }

        public void AddTransfer(T tranfer)
        {
            _transfers.Add(tranfer);
        }
        
        public Result<IEnumerable<TransferGroup<T>>> GetTransferGroups(IInstrument instrument, ITransferOptimizer<T> optimizer)
        {
            var transferGroups = optimizer.OptimizeTransfers(_transfers, instrument);
            return transferGroups;
        }

    }
}