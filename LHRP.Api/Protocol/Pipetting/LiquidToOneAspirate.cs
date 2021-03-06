﻿using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.LiquidTransfers;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Compilation;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Protocol.Pipetting
{
    public class LiquidToOneAspirate : IPipettingCommand
    {
        public Guid CommandId { get; private set; }
        private AspirateParameters _parameters;
        public ChannelPattern<LiquidToOneTransfer> TransferGroup { get; private set; }
        public int RetryCount { get; private set; }
        public ResourcesUsage ResourcesUsed { get; private set; }

        public LiquidToOneAspirate(AspirateParameters parameters,
            ChannelPattern<LiquidToOneTransfer> transferGroup,
            int retryAttempt = 0)
        {
            _parameters = parameters;
            TransferGroup = transferGroup;
            CommandId = Guid.NewGuid();
            RetryCount = retryAttempt;

            ResourcesUsed = new ResourcesUsage();
            foreach (var target in TransferGroup.GetActiveChannels())
            {
                ResourcesUsed.AddConsumableLiquidUsage(target.Source, target.Target.Volume);
            }
        }


        public void ApplyChannelMask(ChannelPattern channelPattern)
        {
            TransferGroup.Mask(channelPattern);
        }

        public Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            return Result.Ok<IEnumerable<IRunnableCommand>>(new List<IRunnableCommand>() { this });
        }

        public ProcessResult Run(IRuntimeEngine engine)
        {
            var pipettor = engine.Instrument.Pipettor;
            var liquidManager = engine.Instrument.LiquidManager;

            List<RuntimeError> errors;
            var transferContext = TransferGroup.ToChannelPatternPipettingContext(_parameters, engine.Instrument, out errors);
            if (errors.Any())
            {
                return new ProcessResult(errors.ToArray());
            }

            var processResult = pipettor.Aspirate(new AspirateContext(transferContext,  _parameters));

            return processResult;
        }

        public ResourcesUsage CalculateResources(IRuntimeEngine engine)
        {
            return ResourcesUsed;
        }
    }
}
