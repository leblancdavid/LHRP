﻿using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol.Transfers.Liquid
{
    public class DefaultLiquidTransferOptimizer : ITransferOptimizer<LiquidTransfer>
    {
        public Result<IEnumerable<TransferGroup<LiquidTransfer>>> OptimizeTransfers(
            IEnumerable<LiquidTransfer> transfers,
            IInstrument instrument)
        {
            var pipettor = instrument.Pipettor;
            var transferGroups = new List<TransferGroup<LiquidTransfer>>();
            var currentTransferGroup = new TransferGroup<LiquidTransfer>(pipettor.Specification.NumChannels);

            foreach (var transfer in transfers)
            {
                bool assigned = false;
                foreach (var group in transferGroups)
                {
                    assigned = TryAssignTransferToGroup(transfer, group, instrument);
                }
                if (!assigned)
                {
                    var newGroup = new TransferGroup<LiquidTransfer>(pipettor.Specification.NumChannels);
                    if (!TryAssignTransferToGroup(transfer, newGroup, instrument))
                    {
                        return Result.Failure<IEnumerable<TransferGroup<LiquidTransfer>>>("Unable to assign a transfer to a transfer group");
                    }
                    transferGroups.Add(newGroup);
                }
            }
            
            return Result.Ok<IEnumerable<TransferGroup<LiquidTransfer>>>(
                transferGroups.OrderBy(x => x.Transfers.FirstOrDefault(t => t != null).Liquid.AssignedId));
        }

        private bool TryAssignTransferToGroup(LiquidTransfer transfer, TransferGroup<LiquidTransfer> group, IInstrument instrument)
        {
            var pipettor = instrument.Pipettor;
            var destinationCoordinates = instrument.Deck.GetCoordinates(transfer.Target.Address);

            if (group.ChannelPattern.IsFull())
            {
                return false;
            }
            
            var assignedTransfer = group.Transfers.FirstOrDefault(x => x != null);
            if(assignedTransfer != null && assignedTransfer.Liquid != transfer.Liquid)
            {
                return false;
            }

            int channelIndex = 0;
            for (channelIndex = 0; channelIndex < pipettor.Specification.NumChannels; ++channelIndex)
            {
                if (pipettor.Specification[channelIndex].CanReach(destinationCoordinates.Value) &&
                    !group.ChannelPattern[channelIndex])
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
