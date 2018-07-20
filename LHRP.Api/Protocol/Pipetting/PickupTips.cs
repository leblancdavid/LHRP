using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol.Pipetting
{
    public class PickupTips : IRunnable
    {
        private ChannelPattern _pattern;
        private double _desiredTipSize;
        public PickupTips(ChannelPattern pattern, 
            double desireTipSize)
        {
            _pattern = pattern;
            _desiredTipSize = desireTipSize;
        }

        public Process Run(IInstrument instrument)
        {
            var process = new Process();
            var tipManager = instrument.Deck.TipManager;
            var pipettor = instrument.Pipettor;
            var tipsResult = tipManager.RequestTips(_pattern, _desiredTipSize);

            if(tipsResult.IsFailure)
            {
                process.AddError(tipsResult.Error);
                return process;
            }

            var commandResult = pipettor.PickupTips(new TipPickupParameters(tipsResult.Value));
            return commandResult;
        }
    }
}