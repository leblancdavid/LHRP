using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Labware;
using LHRP.Api.Labware.Plates;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Instrument.LiquidManagement
{
    public class LiquidManager : ILiquidManager
    {
        private IDeck _deck;
        private LiquidManagerConfiguration _configuration;
        public LiquidManager(LiquidManagerConfiguration configuration, IDeck deck)
        {
            _deck = deck;
            _configuration = configuration;
        }
        public Result RemoveLiquidFromPosition(LabwareAddress address, double volume)
        {
            var plates = _deck.GetPlates();
            var targetPlate = plates.FirstOrDefault(x => x.PositionId == address.PositionId);
            if(targetPlate == null)
            {
                return Result.Fail($"No plate found in position {address.PositionId}");
            }
            
            var well = targetPlate.GetWell(address);
            if(well.IsFailure)
            {
                return well;
            }

            if(well.Value.Volume < volume)
            {
                return Result.Fail($"Insufficient liquid found in well({well.Value.Address.Row},{well.Value.Address.Column})");
            }
            
            well.Value.Remove(volume);

            return Result.Ok();
        }

        public Result AddLiquidToPosition(LabwareAddress address, Liquid liquidToAssign, double volume)
        {
            var plates = _deck.GetPlates();
            var targetPlate = plates.FirstOrDefault(x => x.PositionId == address.PositionId);
            if(targetPlate == null)
            {
                return Result.Fail($"No plate found in position {address.PositionId}");
            }
            
            var well = targetPlate.GetWell(address);
            if(well.IsFailure)
            {
                return well;
            }

            well.Value.AddLiquid(liquidToAssign, volume);

            return Result.Ok();
        }

        public Result<TransferTarget> RequestLiquid(Liquid liquid, double desiredVolume)
        {
            var plates = _deck.GetPlates();
            var liquidContainers = new List<LiquidContainer>();
            foreach(var plate in plates)
            {
                var containers = plate.GetWellsWithLiquid(liquid);
                foreach(var container in containers)
                {
                    if(container.Volume > desiredVolume)
                    {
                        return Result.Ok(new TransferTarget(container.Address, liquid, desiredVolume, TransferType.Aspirate));
                    }
                }
            }

            return Result.Fail<TransferTarget>($"Insufficient volume {desiredVolume}uL of liquid '{liquid.AssignedId}'");
        }

        public Result ClearLiquidAtPosition(LabwareAddress address)
        {
            var plates = _deck.GetPlates();
            var targetPlate = plates.FirstOrDefault(x => x.PositionId == address.PositionId);
            if (targetPlate == null)
            {
                return Result.Fail($"No plate found in position {address.PositionId}");
            }

            var well = targetPlate.GetWell(address);
            if (well.IsFailure)
            {
                return well;
            }

            well.Value.Clear();
            return Result.Ok();
        }
    }
}