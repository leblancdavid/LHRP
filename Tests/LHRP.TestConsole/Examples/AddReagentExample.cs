﻿using LHRP.Api.Instrument;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol;
using LHRP.Api.Protocol.Steps;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.OneToOne;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Scheduling;
using LHRP.Instrument.SimplePipettor.Runtime;
using LHRP.TestConsole.Examples;
using System;

namespace LHRP.TestConsole
{
    public class TransferSamplesExample : IProtocolExampleRunner
    {
        private IScheduleStream _scheduleStream;
        public TransferSamplesExample(IScheduleStream scheduleStream)
        {
            _scheduleStream = scheduleStream;
        }
        public ProcessResult RunExample()
        {
            var simplePipettorSimulation = new SimplePipettorSimulationEngine();
            simplePipettorSimulation.SimulationSpeedFactor = 10;
            //First setup the deck, add a tip rack and 2 plates
            simplePipettorSimulation.Instrument.Deck.AssignLabware(1, ExampleLabwareCreator.GetTipRack());
            simplePipettorSimulation.Instrument.Deck.AssignLabware(2, ExampleLabwareCreator.GetPlate());
            simplePipettorSimulation.Instrument.Deck.AssignLabware(3, ExampleLabwareCreator.GetPlate());

            //Setup protocol and steps
            var protocol = new Protocol();
            var transferSampleStep = new OneToOneTransferStep(
                new OneToOneTransferStepData(GetOneToOneTransferFor96Wells(2, 3, 50.0),
                300, false));
            protocol.AddStep(transferSampleStep);

            var schedule = protocol.Schedule(simplePipettorSimulation);
            _scheduleStream.Send(schedule);

            return protocol.Run(simplePipettorSimulation);
        }

        TransferPattern<OneToOneTransfer> GetOneToOneTransferFor96Wells(int sourcePositionId, int destinationPositionId, double volume)
        {
            var tp = new TransferPattern<OneToOneTransfer>();
            int rows = 8, cols = 12;
            for (int i = 1; i <= rows; ++i)
            {
                for (int j = 1; j <= cols; ++j)
                {
                    var liquid = new Liquid();
                    var sourceTarget = new TransferTarget(new LabwareAddress(i, j, sourcePositionId), volume, TransferType.Aspirate);
                    var destinationTarget = new TransferTarget(new LabwareAddress(i, j, destinationPositionId), volume, TransferType.Dispense);
                    tp.AddTransfer(new OneToOneTransfer(sourceTarget, destinationTarget));
                }
            }

            return tp;

        }
    }
}
