using System;
using LHRP.Api;
using LHRP.Api.Protocol;
using LHRP.Api.Protocol.Steps;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.OneToOne;
using LHRP.Instrument.SimplePipettor.Instrument;
using LHRP.Instrument.SimplePipettor.Runtime;

namespace LHRP.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup protocol and steps
            var protocol = new Protocol();
            var transferSampleStep = new TransferSamplesStep(
                new TransferSamplesStepData(new TransferPattern<OneToOneTransfer>(), 300.0, false));
            protocol.AddStep(transferSampleStep);

            var simplePipettorSimulation = new SimplePipettorSimulationEngine();

            simplePipettorSimulation.Instrument.Deck.AssignLabware(1, LabwareCreator.GetTipRack());
            var schedule = simplePipettorSimulation.Schedule(protocol);
            simplePipettorSimulation.SimulationSpeedFactor = 5;
            simplePipettorSimulation.Run(transferSampleStep);

            //can also schedule or run an individual step
            //nimbusLiteSimulation.Run(transferSampleStep);
        }
    }
}
