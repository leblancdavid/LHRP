using LHRP.Api.Instrument;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol;
using LHRP.Api.Protocol.Steps;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Protocol.Transfers.LiquidTransfers;
using LHRP.Api.Protocol.Transfers.OneToOne;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Scheduling;
using LHRP.Instrument.SimplePipettor.Runtime;
using LHRP.TestConsole.Examples;
using System;

namespace LHRP.TestConsole
{
    public class AddReagentExample : IProtocolExampleRunner
    {
        private IScheduleStream _scheduleStream;
        public AddReagentExample(IScheduleStream scheduleStream)
        {
            _scheduleStream = scheduleStream;
        }
        public ProcessResult RunExample()
        {
            var simplePipettorSimulation = new SimplePipettorSimulationEngine();
            simplePipettorSimulation.SimulationSpeedFactor = 10;
            var deck = simplePipettorSimulation.Instrument.Deck;
            //First setup the deck, add a tip rack and 2 plates

            
            var reagent = new Liquid("TestReagent");

            var reagentTrough = ExampleLabwareCreator.GetReagentPlate1();
            reagentTrough.GetWell(new LabwareAddress(1, 1))?.AssignLiquid(reagent);

            deck.AssignLabware(1, ExampleLabwareCreator.GetTipRack());
            deck.AssignLabware(2, reagentTrough);
            deck.AssignLabware(3, ExampleLabwareCreator.GetPlate());
            //Setup protocol and steps
            var protocol = new Protocol();
            var addReagent = new LiquidTransferStep(
                new LiquidTransferStepData(GetLiquidTransferFor96Wells(reagent, 3, 50.0), reagent,
                300, false, true));
            protocol.AddStep(addReagent);

            var schedule = protocol.Schedule(simplePipettorSimulation, true);
            _scheduleStream.Send(schedule.Value);

            return protocol.Run(simplePipettorSimulation);
        }

        TransferPattern<LiquidToOneTransfer> GetLiquidTransferFor96Wells(Liquid sourceLiquid, int destinationPositionId, double volume)
        {
            var tp = new TransferPattern<LiquidToOneTransfer>();
            int rows = 8, cols = 12;
            for (int i = 1; i <= rows; ++i)
            {
                for (int j = 1; j <= cols; ++j)
                {
                    var destinationTarget = new TransferTarget(new LabwareAddress(i, j, destinationPositionId), volume, TransferType.Dispense);
                    tp.AddTransfer(new LiquidToOneTransfer(sourceLiquid, destinationTarget));
                }
            }

            return tp;

        }
    }
}
