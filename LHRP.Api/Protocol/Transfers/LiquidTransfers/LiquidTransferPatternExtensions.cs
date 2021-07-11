using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime.ErrorHandling;
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

        public static ChannelPattern<ChannelPipettingContext> ToChannelPatternPipettingContext(
            this ChannelPattern<LiquidToOneTransfer> transferPattern,
            IInstrument instrument,
            out List<RuntimeError> errors)
        {
            errors = new List<RuntimeError>();

            var volumeUsagePerLiquid = new Dictionary<string, double>();
            foreach (var liquidTarget in transferPattern.GetActiveChannels())
            {
                var id = liquidTarget.Source.GetId();
                if(!volumeUsagePerLiquid.ContainsKey(id))
                {
                    volumeUsagePerLiquid[id] = 0.0;
                }

                volumeUsagePerLiquid[id] += liquidTarget.Target.Volume;
            }

            var transferContext = new ChannelPattern<ChannelPipettingContext>(transferPattern.NumChannels);
            for (int i = 0; i < transferPattern.NumChannels; ++i)
            {
                if (transferPattern[i] == null)
                {
                    continue;
                }

                var transfer = transferPattern[i]!;

                var transferTarget = instrument.LiquidManager.RequestLiquid(transfer.Source, volumeUsagePerLiquid[transfer.Source.GetId()]);
                //If this happens then there's not enough liquid
                if (transferTarget.IsFailure)
                {
                    var errorMessage = transferTarget.Error;
                    if(!errors.Any(x => x.Message == errorMessage)) //avoid duplicates
                    {
                        errors.Add(new InsufficientLiquidRuntimeError(
                            errorMessage,
                            transfer.Source,
                            volumeUsagePerLiquid[transfer.Source.GetId()]));
                    }

                    continue;
                }

                transferContext[i] = new ChannelPipettingContext(transfer.Target.Volume, i, transfer.Source,
                    transferTarget.Value.AbsolutePosition, transferTarget.Value.Address);
            }

            return transferContext;
        }

        public static ChannelPattern<TransferTarget> ToTargetTransfer(
            this ChannelPattern<LiquidToOneTransfer> channelPattern)
        {
            var output = new ChannelPattern<TransferTarget>(channelPattern.NumChannels);
            for (int i = 0; i < channelPattern.NumChannels; ++i)
            {
                if (channelPattern[i] != null)
                {
                    output[i] = channelPattern[i]!.Target;
                }
            }

            return output;
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

    }
}
