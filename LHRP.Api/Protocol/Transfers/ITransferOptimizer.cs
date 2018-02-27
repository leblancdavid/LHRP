using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol.Transfers
{
    public interface ITransferOptimizer
    {
        Result<IEnumerable<TransferGroup>> OptimizeTransfers(IEnumerable<Transfer> transfers, IInstrument instrument);
    }
}