using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol.Pipetting
{
    public class DropTips : IRunnable
    {
        private bool _returnToSource;
        public DropTips(bool returnToSource=false)
        {
            _returnToSource = returnToSource;
        }

        public Process Run(IInstrument instrument)
        {
            var process = new Process();
            var pipettor = instrument.GetPipettor();
            
            TipDropParameters parameters;
            if(_returnToSource)
            {
                var tipPattern = new TipChannelPattern(pipettor.Specification.NumChannels);
                var pipettorStatus = (PipettorStatus)pipettor.DeviceStatus;
                for(int i = 0; i < pipettor.Specification.NumChannels; ++i)
                {
                    if(pipettorStatus[i].HasTip)
                    {
                        tipPattern.SetTip(i, pipettorStatus[i].CurrentTip);
                    }
                    else
                    {
                        tipPattern[i] = false;
                    }
                }

                parameters = new TipDropParameters(tipPattern);
            } 
            else
            {
                parameters = new TipDropParameters(instrument.WastePosition);
            }

            return pipettor.DropTips(parameters);
        }
    }
}