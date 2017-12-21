using System;
using LHRP.Api;
using LHRP.Api.Protocol;
using LHRP.Instrument.NimbusLite.Instrument;
using LHRP.Instrument.NimbusLite.Runtime;

namespace LHRP.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup protocol and steps
            var protocol = new Protocol();
            var transferSampleStep = new TransferSamplesStep();
            protocol.AddStep(transferSampleStep);

            var nimbusLiteSimulation = new NimbusLiteSimulationEngine();
            nimbusLiteSimulation.Run(protocol);
            
            //can also schedule or run an individual step
            //nimbusLiteSimulation.Run(transferSampleStep);
        }
    }
}
