using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Scheduling;
using System;

namespace LHRP.Api.Protocol.Pipetting
{
    public class DropTips : IRunnableCommand
    {
        private bool _returnToSource;
        public DropTips(bool returnToSource=false)
        {
            _returnToSource = returnToSource;
            CommandId = Guid.NewGuid();
        }

        public Guid CommandId { get; private set; }

        public ProcessResult Run(IRuntimeEngine engine)
        {
            var process = new ProcessResult();
            var pipettor = engine.Instrument.Pipettor;
            
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
                parameters = new TipDropParameters(engine.Instrument.WastePosition);
            }

            return pipettor.DropTips(parameters);
        }

        public Schedule Schedule(IRuntimeEngine runtimeEngine)
        {
            var schedule = new Schedule();

            //Todo: come up with a way to calculate time
            schedule.ExpectedDuration = new TimeSpan(0, 0, 3);
            //TODO
            return schedule;
        }
    }
}