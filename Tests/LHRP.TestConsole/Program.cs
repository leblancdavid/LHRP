using System;
using LHRP.Api;
using LHRP.Api.Protocol;
using LHRP.Api.Protocol.Steps;
using LHRP.Api.Protocol.Transfers;
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
            var transferSampleStep = new TransferSamplesStep(
                new TransferSamplesStepData(new TransferPattern<Transfer>(), 300.0, false),
                new DefaultTransferOptimizer<Transfer>());
            protocol.AddStep(transferSampleStep);

            var nimbusLiteSimulation = new NimbusLiteSimulationEngine();

            nimbusLiteSimulation.Instrument.Deck.AssignLabware(1, LabwareCreator.GetTipRack());
            var schedule = nimbusLiteSimulation.Schedule(protocol);
            nimbusLiteSimulation.SimulationSpeedFactor = 5;
            nimbusLiteSimulation.Run(transferSampleStep);

            //can also schedule or run an individual step
            //nimbusLiteSimulation.Run(transferSampleStep);
        }
    }
}
