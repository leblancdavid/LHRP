using System;
using LHRP.Api;
using LHRP.Api.Protocol;
using LHRP.Instrument.NimbusLite.Instrument;

namespace LHRP.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup the instrument
            var numbusLiteInstrument = new NimbusLiteInstrument();
            
            //Setup protocol and steps
            var protocol = new Protocol();
            var transferSampleStep = new TransferSamplesStep(numbusLiteInstrument.GetPipettor());
            protocol.AddStep(transferSampleStep);

            var protocolSchedule = numbusLiteInstrument.Schedule(protocol);
            numbusLiteInstrument.Run(protocol);
        }
    }
}
