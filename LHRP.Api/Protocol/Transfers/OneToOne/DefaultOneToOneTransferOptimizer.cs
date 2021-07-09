using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol.Transfers.OneToOne
{
    public class DefaultOneToOneTransferOptimizer : ITransferOptimizer<OneToOneTransfer>
    {
        public DefaultOneToOneTransferOptimizer()
        {

        }
        public Result<IEnumerable<ChannelPattern<OneToOneTransfer>>> OptimizeTransfers(
            IEnumerable<OneToOneTransfer> transfers, 
            IInstrument instrument)
        {
            var pipettor = instrument.Pipettor;
            var transferGroups = new List<ChannelPattern<OneToOneTransfer>>();

            foreach(var transfer in transfers)
            {
                bool assigned = false;
                foreach(var group in transferGroups)
                {
                    assigned = TryAssignTransferToGroup(transfer, group, instrument);
                }
                if(!assigned)
                {
                    var newGroup = new ChannelPattern<OneToOneTransfer>(pipettor.Specification.NumChannels);
                    if(!TryAssignTransferToGroup(transfer, newGroup, instrument))
                    {
                        return Result.Failure<IEnumerable<ChannelPattern<OneToOneTransfer>>>("Unable to assign a transfer to a transfer group");
                    }
                    transferGroups.Add(newGroup);
                }
            }

            return Result.Ok<IEnumerable<ChannelPattern<OneToOneTransfer>>>(transferGroups);
        }

        private bool TryAssignTransferToGroup(OneToOneTransfer transfer, ChannelPattern<OneToOneTransfer> group, IInstrument instrument)
        {
            var pipettor = instrument.Pipettor;
            var sourceCoordinates = instrument.Deck.GetCoordinates(transfer.Source.Address);
            var destinationCoordinates = instrument.Deck.GetCoordinates(transfer.Target.Address);
            
            if(sourceCoordinates == null || destinationCoordinates == null)
            {
                return false;
            }

            if(group.IsFull())
            {
                return false;
            }
            int channelIndex = 0;
            for(channelIndex = 0; channelIndex < pipettor.Specification.NumChannels; ++channelIndex)
            {
                if(pipettor.Specification[channelIndex].CanReach(sourceCoordinates) &&
                    pipettor.Specification[channelIndex].CanReach(destinationCoordinates) &&
                    !group[channelIndex])
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