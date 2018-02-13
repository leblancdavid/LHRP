using System.Collections.Generic;
using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol.TransferPattern
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
        
        public IEnumerable<TransferGroup> GetTransferGroups(IInstrument instrument)
        {
            var transferGroups = new List<TransferGroup>();

            return transferGroups;
        }

    }
}