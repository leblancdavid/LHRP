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
        public static ChannelPattern<ChannelPipettingTransfer> ToChannelPatternPipettingContext(
            this ChannelPattern<TransferTarget> transferPattern,
            ILiquidHandlingParameters parameters,
            IInstrument instrument,
            out List<RuntimeError> errors)
        {
            errors = new List<RuntimeError>();
            var transferContext = new ChannelPattern<ChannelPipettingTransfer>(transferPattern.NumChannels);
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
                    container.Volume < transfer.Volume)
                {
                    errors.Add(new InsufficientSourceVolumeRuntimeError(
                        $"Insufficient liquid for aspirate at {transfer.Address.ToAlphaAddress()}",
                        transfer.Address,
                        container.Volume,
                        transfer.Volume));
                }


                transferContext[i] = new ChannelPipettingTransfer(transfer.Volume, container.Liquid!, parameters, i, container, transfer.TransferType);
            }

            return transferContext;
        }
    }
}
