using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Labware;
using LHRP.Api.Protocol.Pipetting;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime;
using System.Linq;

namespace LHRP.Scripting
{
    public class ProtocolScript
    {
        private IRuntimeEngine _runtimeEngine;
        public void SetRuntimeEngine(IRuntimeEngine runtimeEngine)
        {
            _runtimeEngine = runtimeEngine;
        }

        public void AddLabware(string labwareDefinition, int positionId, int labwareId)
        {
            //TODO labware definitions
            _runtimeEngine.Instrument.Deck.AddLabware(positionId,
                    new Plate(new PlateDefinition("Costar 96",
                new RectangularLabwareShape(0.0, 0.0, 0.0, new Coordinates(0.0, 0.0, 0.0)), 
                new WellDefinition(new CylindricalLabwareShape(4.5, 14.5, new Coordinates())), 8, 12, new Coordinates(86, 127, 14), 9.0), 1));
        }

        public void AddTips(string labwareDefinitinon, int positionId, int tipTypeId)
        {
            //TODO labware definitions
            _runtimeEngine.Instrument.Deck.AddLabware(positionId,
                new TipRack(new TipRackDefinition(300, "300uL Tips", 300.0, false, 8, 12, new Coordinates(9.0, 9.0, 9.0), 9.0), 1));
        }

        public void PickUpTips(int tipTypeId, string channelPattern)
        {
            _runtimeEngine.Commands.Add(new PickupTips(new ChannelPattern(channelPattern), tipTypeId));
        }

        public void Aspirate(int positionId, string addresses, string channelPattern, double volume, string liquid)
        {
            var addressArray = addresses.Split(';', ',');
            var pattern = new ChannelPattern(channelPattern);
            var transferTargets = addressArray.Select(x =>
            {
                if (string.IsNullOrEmpty(x))
                {
                    return null;
                }

                return new TransferTarget(new LabwareAddress(x, positionId), volume, TransferType.Aspirate);
            }).ToList();

            //_runtimeEngine.Commands.Add(new TransferTargetAspirate(new AspirateContext(), transferTargets));
        }

        public void Dispense(int positionId, string addresses, string channelPattern, double volume, string liquid)
        {
            var addressArray = addresses.Split(';', ',');
            var pattern = new ChannelPattern(channelPattern);
            var transferTargets = addressArray.Select(x =>
            {
                if (string.IsNullOrEmpty(x))
                {
                    return null;
                }

                return new TransferTarget(new LabwareAddress(x, positionId), volume, TransferType.Dispense);
            }).ToList();

            //_runtimeEngine.Commands.Add(new Dispense(new DispenseContext(), transferTargets, new ChannelPattern(channelPattern)));

        }

        public void DropTips(bool returnToSource)
        {
            _runtimeEngine.Commands.Add(new DropTips(returnToSource));
        }

        public void Run()
        {
            _runtimeEngine.Run();
        }
    }
}
