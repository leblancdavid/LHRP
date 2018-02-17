using LHRP.Api.Common;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol.Pipetting
{
    public class PickupTips : IRunnable
    {
        private PickupTipsOptions _options;
        public PickupTips(PickupTipsOptions options)
        {

        }

        public Result<Process> Run(IInstrument instrument)
        {
            var process = new Process();
            

            return Result<Process>.Ok(process);
        }
    }
}