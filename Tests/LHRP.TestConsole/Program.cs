using System;
using LHRP.Api;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
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
            var simplePipettorSimulation = new SimplePipettorSimulationEngine();
            
            //First setup the deck, add a tip rack and 2 plates
            simplePipettorSimulation.Instrument.Deck.AssignLabware(1, LabwareCreator.GetTipRack());
            simplePipettorSimulation.Instrument.Deck.AssignLabware(2, LabwareCreator.GetPlate());
            simplePipettorSimulation.Instrument.Deck.AssignLabware(3, LabwareCreator.GetPlate());
            
            //Setup protocol and steps
            var protocol = new Protocol();
            var transferSampleStep = new TransferSamplesStep(
                new TransferSamplesStepData(GetOneToOneTransferFor96Wells(2, 3, 50.0), 
                300.0, false));
            protocol.AddStep(transferSampleStep);

            
            var schedule = simplePipettorSimulation.Schedule(protocol);
            simplePipettorSimulation.SimulationSpeedFactor = 5;

            var processResult = protocol.Run(simplePipettorSimulation);

            //can also schedule or run an individual step
            //nimbusLiteSimulation.Run(transferSampleStep);
        }

        static TransferPattern<OneToOneTransfer> GetOneToOneTransferFor96Wells(int sourcePositionId, int destinationPositionId, double volume)
        {
            var tp = new TransferPattern<OneToOneTransfer>();
            int rows = 8, cols = 12;
            for(int i = 1; i <= rows; ++i)
            {
                for(int j = 1; j <= cols; ++j)
                {
                    var liquid = new Liquid();
                    var sourceTarget = new TransferTarget(new LabwareAddress(i, j), sourcePositionId, liquid, volume);
                    var destinationTarget = new TransferTarget(new LabwareAddress(i, j), destinationPositionId, liquid, volume);
                    tp.AddTransfer(new OneToOneTransfer(sourceTarget, destinationTarget));
                }
            }

            return tp;
            
        }
    }
}
