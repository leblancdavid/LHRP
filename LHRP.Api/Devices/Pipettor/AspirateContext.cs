using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Devices.Pipettor
{
    public class AspirateContext
    {
        public ChannelPattern<ChannelPipettingContext> Targets { get; private set; }

        public AspirateContext(ChannelPattern<ChannelPipettingContext> targets, AspirateParameters parameters)
        {
            Targets = targets;
        }

    }
}
