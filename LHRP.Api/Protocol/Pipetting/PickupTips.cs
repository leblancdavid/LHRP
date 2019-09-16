using System;
using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.ErrorHandling.Errors;
using LHRP.Api.Runtime.Scheduling;

namespace LHRP.Api.Protocol.Pipetting
{
    public class PickupTips : IRunnableCommand
    {
        private ChannelPattern _pattern;
        private int _tipTypeId;
        public PickupTips(ChannelPattern pattern, 
            int tipTypeId)
        {
            _pattern = pattern;
            _tipTypeId = tipTypeId;
            CommandId = Guid.NewGuid();
        }
        public Guid CommandId { get; private set; }

        public ProcessResult Run(IRuntimeEngine engine)
        {
            var process = new ProcessResult();
            var tipManager = engine.Instrument.TipManager;
            var pipettor = engine.Instrument.Pipettor;
            var tipsResult = tipManager.RequestTips(_pattern, _tipTypeId);

            if(tipsResult.IsFailure)
            {
                process.AddError(new InsuffientTipsRuntimeError(_tipTypeId));
                return process;
            }

            var commandResult = pipettor.PickupTips(new TipPickupParameters(tipsResult.Value));
            if(!commandResult.ContainsErrors)
            {
                //TODO LOGIC HERE TO RESOLVE PICKUP ERRORS    
            }

            for(int channel = 0; channel < pipettor.PipettorStatus.ChannelStatus.Count(); ++channel)
            {
                if(tipsResult.Value[channel] && pipettor.PipettorStatus[channel].HasTip)
                {
                    var consumeResult = tipManager.ConsumeTip(tipsResult.Value.GetTip(channel));
                    if(consumeResult.IsFailure)
                    {
                        process.AddError(new RuntimeError(""));
                        return process;
                    }
                }
            }

           
            return commandResult;
        }

        public Schedule Schedule(IRuntimeEngine runtimeEngine)
        {
            var schedule = new Schedule();
            schedule.ResourcesUsage.AddTipUsage(_tipTypeId, _pattern.GetNumberActiveChannels());

            //Todo: come up with a way to calculate time
            schedule.ExpectedDuration = new TimeSpan(0, 0, 4);

            return schedule;
        }
    }
}