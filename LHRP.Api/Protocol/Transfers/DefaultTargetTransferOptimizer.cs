using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol.Transfers
{
    public class DefaultTargetTransferOptimizer<T> : ITransferOptimizer<T> where T : Transfer
    {
        public DefaultTargetTransferOptimizer()
        {

        }
        public Result<IEnumerable<TransferGroup<T>>> OptimizeTransfers(
            IEnumerable<T> transfers, 
            IInstrument instrument)
        {
            var pipettor = instrument.Pipettor;
            var transferGroups = new List<TransferGroup<T>>();
            var currentTransferGroup = new TransferGroup<T>(pipettor.Specification.NumChannels);

            foreach(var transfer in transfers)
            {
                bool assigned = false;
                foreach(var group in transferGroups)
                {
                    assigned = TryAssignTransferToGroup(transfer, group, instrument);
                }
                if(!assigned)
                {
                    var newGroup = new TransferGroup<T>(pipettor.Specification.NumChannels);
                    if(!TryAssignTransferToGroup(transfer, newGroup, instrument))
                    {
                        return Result.Failure<IEnumerable<TransferGroup<T>>>("Unable to assign a transfer to a transfer group");
                    }
                    transferGroups.Add(newGroup);
                }
            }

            return Result.Ok<IEnumerable<TransferGroup<T>>>(transferGroups);
        }

        private bool TryAssignTransferToGroup(T transfer, TransferGroup<T> group, IInstrument instrument)
        {
            var pipettor = instrument.Pipettor;
            var destinationCoordinates = instrument.Deck.GetCoordinates(transfer.Target.Address);
            

            if(group.ChannelPattern.IsFull())
            {
                return false;
            }

            int channelIndex = 0;

            for(channelIndex = 0; channelIndex < pipettor.Specification.NumChannels; ++channelIndex)
            {
                if(pipettor.Specification[channelIndex].CanReach(destinationCoordinates.Value))
                {
                    group[channelIndex] = transfer;
                    channelIndex++;
                    return true;
                }
            }

            return false;
        }
    }
}