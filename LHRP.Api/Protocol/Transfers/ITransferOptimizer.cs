using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol.Transfers
{
    public interface ITransferOptimizer<T> where T: ITransfer
    {
        Result<IEnumerable<TransferGroup<T>>> OptimizeTransfers(IEnumerable<T> transfers, IInstrument instrument);
    }
}