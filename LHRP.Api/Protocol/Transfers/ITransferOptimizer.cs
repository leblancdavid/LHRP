using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol.Transfers
{
    public interface ITransferOptimizer<T> where T: class, ITransfer
    {
        Result<IEnumerable<ChannelPattern<T>>> OptimizeTransfers(IEnumerable<T> transfers, IInstrument instrument);
    }
}