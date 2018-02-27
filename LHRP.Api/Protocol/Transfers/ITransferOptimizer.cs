using System.Collections.Generic;
using LHRP.Api.Common;
using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol.Transfers
{
    public interface ITransferOptimizer
    {
        Result<IEnumerable<TransferGroup>> OptimizeTransfers(IEnumerable<Transfer> transfers, IInstrument instrument);
    }
}