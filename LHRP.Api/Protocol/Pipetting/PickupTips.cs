using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.Resources;

namespace LHRP.Api.Protocol.Pipetting
{
    public class PickupTips : IPipettingCommand
    {
        private ChannelPattern _pattern;
        private int _tipTypeId;
        public ResourcesUsage ResourcesUsed { get; private set; }

        public PickupTips(ChannelPattern pattern, 
            int tipTypeId,
            int retryAttempt = 0)
        {
            _pattern = pattern;
            _tipTypeId = tipTypeId;
            CommandId = Guid.NewGuid();
            RetryCount = retryAttempt;

            ResourcesUsed = new ResourcesUsage();
            ResourcesUsed.AddTipUsage(_tipTypeId, _pattern.GetNumberActiveChannels());
        }
        public Guid CommandId { get; private set; }
        public int RetryCount { get; private set; }

        public void ApplyChannelMask(ChannelPattern channelPattern)
        {
            _pattern = channelPattern;
        }

        public Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            return Result.Ok<IEnumerable<IRunnableCommand>>(new List<IRunnableCommand>() { this });
        }

        public ProcessResult Run(IRuntimeEngine engine)
        {
            var process = new ProcessResult();
            var tipManager = engine.Instrument.TipManager;
            var pipettor = engine.Instrument.Pipettor;
            var tipsResult = tipManager.RequestTips(_pattern, _tipTypeId);

            if(tipsResult.IsFailure)
            {
                process.AddError(new InsuffientTipsRuntimeError(tipsResult.Error, _tipTypeId));
                return process;
            }

            var commandResult = pipettor.PickupTips(new TipPickupParameters(tipsResult.Value, _tipTypeId));
            if(commandResult.ContainsErrors)
            {
                foreach(var error in commandResult.Errors)
                {
                    process.AddError(error);
                }
                return process;
            }

            for(int channel = 0; channel < pipettor.PipettorStatus.ChannelStatus.Count(); ++channel)
            {
                if(tipsResult.Value.IsInUse(channel) && pipettor.PipettorStatus[channel].HasTip)
                {
                    var consumeResult = tipManager.ConsumeTip(tipsResult.Value.GetTip(channel)!);
                    if(consumeResult.IsFailure)
                    {
                        process.AddError(new RuntimeError($"Unable to consume tips: '{consumeResult.Error}'"));
                        return process;
                    }
                }
            }

           
            return commandResult;
        }

        public ResourcesUsage CalculateResources(IRuntimeEngine engine)
        {
            return ResourcesUsed;
        }
    }
}