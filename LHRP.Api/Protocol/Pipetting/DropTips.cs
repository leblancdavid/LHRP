using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol.Pipetting
{
    public class DropTips : IRunnable
    {
        private TipChannelPattern _tipPattern;
        public DropTips(TipChannelPattern tipPattern)
        {
            
        }

        public Result<Process> Run(IInstrument instrument)
        {
            throw new System.NotImplementedException();
        }
    }
}