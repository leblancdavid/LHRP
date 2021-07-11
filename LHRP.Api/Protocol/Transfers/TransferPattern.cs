using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol.Transfers
{
    public class TransferPattern<T> where T : class, ITransfer
    {
        protected List<T> _transfers = new List<T>();
        public IEnumerable<T> Transfers => _transfers;

        public TransferPattern()
        {
            _transfers = new List<T>();
        }

        public TransferPattern(List<T> transfers)
        {
            _transfers = transfers;
        }

        virtual public Result AddTransfer(T tranfer)
        {
            _transfers.Add(tranfer);
            return Result.Ok();
        }
        
        public Result<IEnumerable<ChannelPattern<T>>> GetTransferGroups(IInstrument instrument, ITransferOptimizer<T> optimizer)
        {
            var transferGroups = optimizer.OptimizeTransfers(_transfers, instrument);
            return transferGroups;
        }

    }
}