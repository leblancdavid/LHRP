using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Protocol.Transfers
{
    public static class TransferTargetExtensions
    {
        public static ChannelPattern<ChannelPipettingContext> ToChannelPatternPipettingContext(
            this ChannelPattern<TransferTarget> transferPattern,
            IInstrument instrument,
            out List<RuntimeError> errors)
        {
            errors = new List<RuntimeError>();
            var transferContext = new ChannelPattern<ChannelPipettingContext>(transferPattern.NumChannels);
            for (int i = 0; i < transferPattern.NumChannels; ++i)
            {
                if(transferPattern[i] == null)
                {
                    continue;
                }

                var transfer = transferPattern[i]!;
                var container = instrument.Deck.GetLiquidContainer(transfer.Address);
                if(container == null)
                {
                    errors.Add(new RuntimeError($"Invalid transfer target address {transfer.Address.ToAlphaAddress()}"));
                    continue;
                }

                if(transfer.TransferType == TransferType.Aspirate &&
                    container.Liquid != null && 
                    container.Volume < transfer.Volume)
                {
                    errors.Add(new InsufficientSourceVolumeRuntimeError(
                        $"Invalid transfer target address {transfer.Address.ToAlphaAddress()}",
                        transfer.Address,
                        container.Volume,
                        transfer.Volume));
                }

                transferContext[i] = new ChannelPipettingContext(transfer.Volume, i, container.Liquid!,
                    container.AbsolutePosition, transfer.Address);
            }

            return transferContext;
        }
    }
}
