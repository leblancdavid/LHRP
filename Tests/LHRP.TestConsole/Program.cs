using System;
using LHRP.Api;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol;
using LHRP.Api.Protocol.Steps;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.OneToOne;
using LHRP.Api.Runtime.Scheduling;
using LHRP.Instrument.SimplePipettor.Instrument;
using LHRP.Instrument.SimplePipettor.Runtime;

namespace LHRP.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var simplePipettorSimulation = new SimplePipettorSimulationEngine();
            simplePipettorSimulation.SimulationSpeedFactor = 10;
            //First setup the deck, add a tip rack and 2 plates
            simplePipettorSimulation.Instrument.Deck.AssignLabware(1, LabwareCreator.GetTipRack());
            simplePipettorSimulation.Instrument.Deck.AssignLabware(2, LabwareCreator.GetPlate());
            simplePipettorSimulation.Instrument.Deck.AssignLabware(3, LabwareCreator.GetPlate());
            
            //Setup protocol and steps
            var protocol = new Protocol();
            var transferSampleStep = new OneToOneTransferStep(
                new OneToOneTransferStepData(GetOneToOneTransferFor96Wells(2, 3, 50.0), 
                0, false));
            protocol.AddStep(transferSampleStep);
            var transferSampleBackStep = new OneToOneTransferStep(
                new OneToOneTransferStepData(GetOneToOneTransferFor96Wells(3, 2, 45.0),
                0, false));
            protocol.AddStep(transferSampleBackStep);
            
            //var schedule = protocol.Schedule(simplePipettorSimulation);
            //PrintSchedule(schedule);

            var processResult = protocol.Run(simplePipettorSimulation);

            Console.WriteLine("Done... Hit enter key to continue.");
            Console.ReadLine();
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
                    var sourceTarget = new TransferTarget(new LabwareAddress(i, j, sourcePositionId), liquid, volume, TransferType.Aspirate);
                    var destinationTarget = new TransferTarget(new LabwareAddress(i, j, destinationPositionId), liquid, volume, TransferType.Dispense);
                    tp.AddTransfer(new OneToOneTransfer(sourceTarget, destinationTarget));
                }
            }

            return tp;
            
        }

        static void PrintSchedule(Schedule schedule)
        {
            Console.WriteLine("Usage in liquid containers:");
            foreach(var liquidUsage in schedule.ResourcesUsage.LiquidContainerUsages)
            {
                Console.WriteLine($"Pos{liquidUsage.Address.PositionId}-{liquidUsage.Address.ToAlphaAddress()}, Starting liquid volume: {liquidUsage.RequiredLiquidVolumeAtStart}uL, Ending volume: {liquidUsage.ExpectedFinalLiquidVolume}uL");
                foreach(var transfer in liquidUsage.TransferHistory)
                {
                    Console.WriteLine($"{transfer.TransferType} {transfer.Volume}uL");
                }
            }
            Console.WriteLine("Tip usage:");
            foreach (var tipUsage in schedule.ResourcesUsage.TipUsages)
            {
                Console.WriteLine($"Total tips ({tipUsage.TipTypeId}) used: {tipUsage.ExpectedTotalTipUsage}");
            }
            
            Console.WriteLine($"Estimated total run-time: {schedule.ExpectedDuration.ToString("c")}");

            Console.ReadLine();
        }
    }
}
