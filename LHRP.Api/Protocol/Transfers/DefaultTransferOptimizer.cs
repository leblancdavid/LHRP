using System.Collections.Generic;
using System.Linq;
using LHRP.Api.Common;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol.Transfers
{
    public class DefaultTransferOptimizer : ITransferOptimizer
    {
        public DefaultTransferOptimizer()
        {

        }
        public Result<IEnumerable<TransferGroup>> OptimizeTransfers(
            IEnumerable<Transfer> transfers, 
            IInstrument instrument)
        {
            var pipettor = instrument.GetPipettor();
            var transferGroups = new List<TransferGroup>();
            var currentTransferGroup = new TransferGroup(pipettor.Specification.NumChannels);

            foreach(var transfer in transfers)
            {
                bool assigned = false;
                foreach(var group in transferGroups)
                {
                    assigned = TryAssignTransferToGroup(transfer, group, instrument);
                }
                if(!assigned)
                {
                    var newGroup = new TransferGroup(pipettor.Specification.NumChannels);
                    if(!TryAssignTransferToGroup(transfer, newGroup, instrument))
                    {
                        return Result<IEnumerable<TransferGroup>>.Fail("Unable to assign a transfer to a transfer group");
                    }
                    transferGroups.Add(newGroup);
                }
            }

            return Result<IEnumerable<TransferGroup>>.Ok(transferGroups);
        }

        private bool TryAssignTransferToGroup(Transfer transfer, TransferGroup group, IInstrument instrument)
        {
            var pipettor = instrument.GetPipettor();
            var sourceCoordinates = instrument.Deck.GetCoordinates(transfer.Source.PositionId, transfer.Source.Address);
            var destinationCoordinates = instrument.Deck.GetCoordinates(transfer.Destination.PositionId, transfer.Destination.Address);
            

            if(group.ChannelPattern.IsFull())
            {
                return false;
            }
            int channelIndex = 0;
            for(channelIndex = 0; channelIndex < pipettor.Specification.NumChannels; ++channelIndex)
            {
                if(pipettor.Specification[channelIndex].CanReach(sourceCoordinates.Value) &&
                    pipettor.Specification[channelIndex].CanReach(destinationCoordinates.Value))
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