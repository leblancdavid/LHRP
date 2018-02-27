using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol.Pipetting
{
    public class PickupTips : IRunnable
    {
        private PickupTipsOptions _options;
        public PickupTips(PickupTipsOptions options)
        {
            _options = options;
        }

        public Result<Process> Run(IInstrument instrument)
        {
            var process = new Process();
            var tipManager = instrument.Deck.TipManager;
            var pipettor = instrument.GetPipettor();
            var tips = tipManager.RequestTips(_options.Pattern, _options.DesiredTipSize);

            
            return Result.Ok<Process>(process);
        }
    }
}