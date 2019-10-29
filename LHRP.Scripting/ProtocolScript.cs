using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware;
using LHRP.Api.Labware.Plates;
using LHRP.Api.Labware.Tips;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Pipetting;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime;
using System.Linq;

namespace LHRP.Scripting
{
    public static class ProtocolScript
    {
        private static IRuntimeEngine _runtimeEngine;
        private static void SetRuntimeEngine(IRuntimeEngine runtimeEngine)
        {
            _runtimeEngine = runtimeEngine;
        }

        private static void AddLabware(string labwareDefinition, int positionId, int labwareId)
        {
            //TODO labware definitions
            _runtimeEngine.Instrument.Deck.AssignLabware(positionId,
                    new Plate(new PlateDefinition("Costar 96", new WellDefinition(), 8, 12, new Coordinates(86, 127, 14), 9.0)));
        }

        private static void AddTips(string labwareDefinitinon, int positionId, int tipTypeId)
        {
            //TODO labware definitions
            _runtimeEngine.Instrument.Deck.AssignLabware(positionId,
                new TipRack(new TipRackDefinition("300uL Tips", 300.0, false, 8, 12, new Coordinates(9.0, 9.0, 9.0), 9.0)));
        }

        private static void PickUpTips(int tipTypeId, string channelPattern)
        {
            _runtimeEngine.Commands.Add(new PickupTips(new ChannelPattern(channelPattern), tipTypeId));
        }

        private static void Aspirate(int positionId, string addresses, string channelPattern, double volume, string liquid)
        {
            var addressArray = addresses.Split(';', ',');
            var pattern = new ChannelPattern(channelPattern);
            var transferTargets = addressArray.Select(x =>
            {
                if (string.IsNullOrEmpty(x))
                {
                    return null;
                }

                return new TransferTarget(new LabwareAddress(x, positionId), new Liquid(), volume, TransferType.Aspirate);
            }).ToList();

            _runtimeEngine.Commands.Add(new TransferTargetAspirate(new AspirateParameters(), transferTargets, new ChannelPattern(channelPattern)));
        }

        private static void Dispense(int positionId, string addresses, string channelPattern, double volume, string liquid)
        {
            var addressArray = addresses.Split(';', ',');
            var pattern = new ChannelPattern(channelPattern);
            var transferTargets = addressArray.Select(x =>
            {
                if (string.IsNullOrEmpty(x))
                {
                    return null;
                }

                return new TransferTarget(new LabwareAddress(x, positionId), new Liquid(), volume, TransferType.Dispense);
            }).ToList();

            _runtimeEngine.Commands.Add(new Dispense(new DispenseParameters(), transferTargets, new ChannelPattern(channelPattern)));

        }

        private static void DropTips(bool returnToSource)
        {
            _runtimeEngine.Commands.Add(new DropTips(returnToSource));
        }

        private static void Run()
        {
            _runtimeEngine.Run();
        }
    }
}
