using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Devices.Pipettor
{
    public class InMemoryPipetteLogger : IPipetteLogger
    {
        private ChannelPattern<PipetteSequenceLog>? _currentSequence;
        private List<PipetteSequenceLog> _sequences = new List<PipetteSequenceLog>();
        public InMemoryPipetteLogger()
        {

        }

        public void BeginSequence(ChannelPattern pattern)
        {
            if(_currentSequence == null)
            {
                _currentSequence = new ChannelPattern<PipetteSequenceLog>(pattern.NumChannels);
            }
            for(int i = 0; i < pattern.NumChannels; ++i)
            {
                if(pattern.IsInUse(i))
                {
                    _currentSequence[i] = new PipetteSequenceLog();
                }
            }

        }

        public void EndSequence(ChannelPattern pattern)
        {
            if (_currentSequence == null)
            {
                return;
            }
            for (int i = 0; i < pattern.NumChannels; ++i)
            {
                if (pattern.IsInUse(i) && _currentSequence[i] != null)
                {
                    _sequences.Add(_currentSequence[i]!);
                }
            }
        }

        public PipetteSequenceLog GetLiquidTracking(Liquid? liquid = null, LabwareAddress? address = null)
        {
            var filtered = new List<PipetteSequenceLog>();
            foreach(var sequence in _sequences)
            {
                if(liquid != null && !sequence.HasTransferedLiquid(liquid))
                {
                    continue;
                }

                if(address != null && !sequence.HasTransferedFrom(address))
                {
                    continue;
                }

                filtered.Add(sequence);
            }

            return PipetteSequenceLog.Combine(filtered.ToArray());
        }

        public IEnumerable<ChannelPipettingTransfer> GetSourceTransfers()
        {
            var allTransfers = PipetteSequenceLog.Combine(_sequences.ToArray()).Transfers.ToList();
            var sourceTransfers = new List<ChannelPipettingTransfer>();
            for(int i = 0; i < allTransfers.Count; ++i)
            {
                if(allTransfers[i].Transfer == TransferType.Aspirate &&
                    !allTransfers.Take(i).Any(x => x.Address == allTransfers[i].Address))
                {
                    sourceTransfers.Add(allTransfers[i]);
                }
            }

            return sourceTransfers;
        }

        public void LogTransfer(ChannelPattern<ChannelPipettingTransfer> transfer)
        {
            if (_currentSequence == null || _currentSequence.NumChannels != transfer.NumChannels)
            {
                return;
            }

            for (int i = 0; i < transfer.NumChannels; ++i)
            {
                if (transfer.IsInUse(i) && _currentSequence[i] != null)
                {
                    _currentSequence[i]!.Add(transfer[i]!);
                }
            }
        }

        public void Reset()
        {
            
        }


    }
}
