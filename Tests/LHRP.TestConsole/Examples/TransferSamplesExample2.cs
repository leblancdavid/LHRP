using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware;
using LHRP.Api.Protocol;
using LHRP.Api.Protocol.Steps;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.OneToOne;
using LHRP.Api.Runtime;
using LHRP.Instrument.SimplePipettor.Runtime;
using LHRP.TestConsole.Examples;
using System;

namespace LHRP.TestConsole
{
    public class TransferSamplesExample2 : IProtocolExampleRunner
    {
        public TransferSamplesExample2()
        {
        }
        public ProcessResult RunExample()
        {
            var simplePipettorSimulation = new SimplePipettorSimulationEngine();
            //simplePipettorSimulation.SimulationSpeedFactor = 10;
            //First setup the deck, add a tip rack and 2 plates
            simplePipettorSimulation.Instrument.Deck.AddLabware(3, ExampleLabwareCreator.GetTipRack(3));
            simplePipettorSimulation.Instrument.Deck.AddLabware(1, ExampleLabwareCreator.GetPlate(1));
            simplePipettorSimulation.Instrument.Deck.AddLabware(2, ExampleLabwareCreator.GetPlate(2));

            //Setup protocol and steps
            var protocol = new Protocol();
            var transferSampleStep = new OneToOneTransferStep(
                new OneToOneTransferStepData(GetOneToOneTransferFor96Wells(1, 2, 150.0),
                //Aspirate from the liquid level -1mm
                new AspirateParameters(pipettingHeight: -1.0, pipettePositionType: PipettePositionType.FromLiquidLevel),
                //Dispense to 5mm above the liquid level
                new DispenseParameters(pipettingHeight: 5.0, pipettePositionType: PipettePositionType.FromLiquidLevel),
                300, false));
            //Repeat the step twice so that we can see how the aspirate and dispense positions change
            protocol.AddStep(transferSampleStep);
            protocol.AddStep(transferSampleStep);

            return protocol.Run(simplePipettorSimulation);
        }

        TransferPattern<OneToOneTransfer> GetOneToOneTransferFor96Wells(int sourcePositionId, int destinationPositionId, double volume)
        {
            var tp = new TransferPattern<OneToOneTransfer>();
            int rows = 2, cols = 1;
            for (int i = 1; i <= rows; ++i)
            {
                for (int j = 1; j <= cols; ++j)
                {
                    var sourceTarget = new TransferTarget(new LabwareAddress(i, j, sourcePositionId), volume, TransferType.Aspirate);
                    var destinationTarget = new TransferTarget(new LabwareAddress(i, j, destinationPositionId), volume, TransferType.Dispense);
                    tp.AddTransfer(new OneToOneTransfer(sourceTarget, destinationTarget));
                }
            }

            return tp;

        }
    }
}
