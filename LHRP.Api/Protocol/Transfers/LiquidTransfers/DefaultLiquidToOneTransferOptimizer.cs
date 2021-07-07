using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Protocol.Transfers.LiquidTransfers
{
    public class DefaultLiquidToOneTransferOptimizer : ITransferOptimizer<LiquidToOneTransfer>
    {
        public DefaultLiquidToOneTransferOptimizer()
        {

        }
        public Result<IEnumerable<TransferGroup<LiquidToOneTransfer>>> OptimizeTransfers(
            IEnumerable<LiquidToOneTransfer> transfers,
            IInstrument instrument)
        {
            var pipettor = instrument.Pipettor;
            var transferGroups = new List<TransferGroup<LiquidToOneTransfer>>();
            var currentTransferGroup = new TransferGroup<LiquidToOneTransfer>(pipettor.Specification.NumChannels);

            foreach (var transfer in transfers)
            {
                bool assigned = false;
                foreach (var group in transferGroups)
                {
                    assigned = TryAssignTransferToGroup(transfer, group, instrument);
                }
                if (!assigned)
                {
                    var newGroup = new TransferGroup<LiquidToOneTransfer>(pipettor.Specification.NumChannels);
                    if (!TryAssignTransferToGroup(transfer, newGroup, instrument))
                    {
                        return Result.Failure<IEnumerable<TransferGroup<LiquidToOneTransfer>>>("Unable to assign a transfer to a transfer group");
                    }
                    transferGroups.Add(newGroup);
                }
            }

            return Result.Ok<IEnumerable<TransferGroup<LiquidToOneTransfer>>>(transferGroups);
        }

        private bool TryAssignTransferToGroup(LiquidToOneTransfer transfer, TransferGroup<LiquidToOneTransfer> group, IInstrument instrument)
        {
            var pipettor = instrument.Pipettor;
            var destinationCoordinates = instrument.Deck.GetCoordinates(transfer.Target.Address);
            if(destinationCoordinates == null)
            {
                return false;
            }

            if (group.ChannelPattern.IsFull())
            {
                return false;
            }

            int channelIndex = 0;

            for (channelIndex = 0; channelIndex < pipettor.Specification.NumChannels; ++channelIndex)
            {
                if (pipettor.Specification[channelIndex].CanReach(destinationCoordinates) && !group.ChannelPattern[channelIndex])
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
