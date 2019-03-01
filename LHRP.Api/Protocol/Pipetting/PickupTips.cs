using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.ErrorHandling.Errors;

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
        }

        public Process Run(IRuntimeEngine engine)
        {
            var process = new Process();
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
    }
}