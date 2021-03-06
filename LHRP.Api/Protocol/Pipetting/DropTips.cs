using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Resources;
using System;
using System.Collections.Generic;

namespace LHRP.Api.Protocol.Pipetting
{
    public class DropTips : IPipettingCommand
    {
        private bool _returnToSource;
        public ResourcesUsage ResourcesUsed { get; private set; }
        public DropTips(bool returnToSource=false,
            int retryAttempt = 0)
        {
            _returnToSource = returnToSource;
            CommandId = Guid.NewGuid();
            RetryCount = retryAttempt;

            ResourcesUsed = new ResourcesUsage();
        }

        public Guid CommandId { get; private set; }
        public int RetryCount { get; private set; }

        public void ApplyChannelMask(ChannelPattern channelPattern)
        {
            //Do nothing
        }

        public Result<IEnumerable<IRunnableCommand>> GetCommands(IRuntimeEngine engine)
        {
            return Result.Ok<IEnumerable<IRunnableCommand>>(new List<IRunnableCommand>() { this });
        }

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
                        tipPattern.SetTip(i, pipettorStatus[i].CurrentTip!);
                    }
                    else
                    {
                        tipPattern[i] = null;
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

        public ResourcesUsage CalculateResources(IRuntimeEngine engine)
        {
            return ResourcesUsed;
        }
    }
}