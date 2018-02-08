using System.Collections.Generic;

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
        

    }
}