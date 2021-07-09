using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Protocol.Transfers.LiquidTransfers
{
    public static class LiquidTransferPatternExtensions
    {
        public static Result<IEnumerable<ChannelPattern<LiquidToManyTransfer?>>> GetMultiDispenseTransferGroups(
            this TransferPattern<LiquidToOneTransfer> transferPattern,
            IInstrument instrument, 
            ITransferOptimizer<LiquidToOneTransfer> optimizer, 
            double maxUsableVolume)
        {
            var multiDispenseTransferGroups = new List<ChannelPattern<LiquidToManyTransfer?>>();

            var liquidToOneTransferGroups = transferPattern.GetTransferGroups(instrument, optimizer);
            if (liquidToOneTransferGroups.IsFailure)
            {
                return Result.Failure<IEnumerable<ChannelPattern<LiquidToManyTransfer?>>>(liquidToOneTransferGroups.Error);
            }
            if (!liquidToOneTransferGroups.Value.Any())
            {
                return Result.Failure<IEnumerable<ChannelPattern<LiquidToManyTransfer?>>>("No liquid transfer groups to process for liquid transfer");
            }
            if (maxUsableVolume < 0.0)
            {
                return Result.Failure<IEnumerable<ChannelPattern<LiquidToManyTransfer?>>>("Insufficient volume to handle multi-dispense transfer");
            }

            var transferList = liquidToOneTransferGroups.Value.ToList();
            var channelPattern = GetOverallChannelPattern(transferList);
            int numChannels = channelPattern.NumChannels;

            for (int i = 0; i < transferList.Count; ++i)
            {
                var mdTransferGroup = new ChannelPattern<LiquidToManyTransfer?>(numChannels);
                for (int j = i; j < transferList.Count; ++j)
                {
                    for (int channel = 0; channel < numChannels; ++channel)
                    {
                        if(transferList[j].IsInUse(channel))
                        {
                            continue;
                        }

                        if(mdTransferGroup[channel] == null)
                        {
                            mdTransferGroup[channel] = new LiquidToManyTransfer(transferList[j][channel]!.Source);
                        }

                        double currentTotal = mdTransferGroup[channel]!.GetTotalTransferVolume();
                        if(transferList[j][channel]!.Target.Volume + currentTotal < maxUsableVolume)
                        {
                            mdTransferGroup[channel]!.AddTransferTarget(transferList[j][channel]!.Target);
                        }
                    }
                }

                for(int channel = 0; channel < numChannels; ++channel)
                {
                    //Remove channels that aren't used
                    if(!mdTransferGroup[channel]!.Targets.Any())
                    {
                        mdTransferGroup[channel] = null;
                    }
                }

                if(!mdTransferGroup.IsEmpty())
                {
                    multiDispenseTransferGroups.Add(mdTransferGroup);
                }
            }

            return Result.Ok<IEnumerable<ChannelPattern<LiquidToManyTransfer?>>>(multiDispenseTransferGroups);
        }

        private static ChannelPattern GetOverallChannelPattern(List<ChannelPattern<LiquidToOneTransfer>> liquidTransferGroups)
        {
            if (!liquidTransferGroups.Any())
            {
                return ChannelPattern.Empty(0);
            }

            var overallChannelPattern = ChannelPattern.Empty(liquidTransferGroups.First().NumChannels);
            foreach (var transfer in liquidTransferGroups)
            {
                //overallChannelPattern = overallChannelPattern + transfer;
            }

            return overallChannelPattern;
        }

        public static List<LiquidTransferSource?> GetAspirateLiquidTargets(this ChannelPattern<LiquidToManyTransfer> transferGroup, double additionalVolume = 0.0)
        {
            var liquidTargets = new List<LiquidTransferSource?>();
            for(int i = 0; i < transferGroup.NumChannels; ++i)
            {
                if(transferGroup[i] != null)
                {
                    liquidTargets.Add(new LiquidTransferSource(transferGroup[i]!.Source,
                        transferGroup[i]!.GetTotalTransferVolume() + additionalVolume));
                }
                else
                {
                    liquidTargets.Add(null);
                }
            }
            return liquidTargets;
        }
    }
}
