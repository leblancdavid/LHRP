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

                var coord = instrument.Deck.GetCoordinates(transferPattern[i]!.Address);
                if(coord == null)
                {
                    errors.Add(new RuntimeError($"Invalid transfer target address {transferPattern[i]!.Address.ToAlphaAddress()}"));
                    
                }

                //transferPattern[i]
            }

            return transferContext;
        }
    }
}
