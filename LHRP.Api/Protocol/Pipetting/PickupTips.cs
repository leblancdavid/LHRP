using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Errors;

namespace LHRP.Api.Protocol.Pipetting
{
    public class PickupTips : IRunnableCommand
    {
        private ChannelPattern _pattern;
        private double _desiredTipSize;
        public PickupTips(ChannelPattern pattern, 
            double desireTipSize)
        {
            _pattern = pattern;
            _desiredTipSize = desireTipSize;
        }

        public Process Run(IRuntimeEngine engine)
        {
            var process = new Process();
            var tipManager = engine.Instrument.TipManager;
            var pipettor = engine.Instrument.Pipettor;
            var tipsResult = tipManager.RequestTips(_pattern, _desiredTipSize);

            if(tipsResult.IsFailure)
            {
                process.AddError(new RuntimeError());
                return process;
            }

            var commandResult = pipettor.PickupTips(new TipPickupParameters(tipsResult.Value));
            return commandResult;
        }
    }
}