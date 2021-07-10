using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Instrument
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
                return Result.Failure($"No plate found in position {address.PositionId}");
            }
            
            var well = targetPlate.GetWell(address);
            if(well == null)
            {
                return Result.Failure($"No well found at address {address.ToAlphaAddress()}");
            }

            if(well.Volume < volume)
            {
                return Result.Failure($"Insufficient liquid found in well({well.Address.Row},{well.Address.Column})");
            }
            
            well.Remove(volume);

            return Result.Ok();
        }

        public Result AddLiquidToPosition(LabwareAddress address, Liquid liquidToAssign, double volume)
        {
            var plates = _deck.GetPlates();
            var targetPlate = plates.FirstOrDefault(x => x.PositionId == address.PositionId);
            if(targetPlate == null)
            {
                return Result.Failure($"No plate found in position {address.PositionId}");
            }
            
            var well = targetPlate.GetWell(address);
            if(well == null)
            {
                return Result.Failure($"No well found at address {address.ToAlphaAddress()}");
            }

            well.AddLiquid(liquidToAssign, volume);

            return Result.Ok();
        }
        public Result AddLiquid(Liquid liquid, double volume)
        {
            var plates = _deck.GetPlates();
            double remainingLiquidVolumeToAssign = volume;
            bool liquidAssignmentFound = false;
            foreach(var plate in plates)
            {
                var wellsWithLiquid = plate.GetWellsWithLiquid(liquid);
                foreach(var well in wellsWithLiquid)
                {
                    if(well.IsPure)
                    {
                        liquidAssignmentFound = true;
                        double availableVolume = well.AvailableVolume;
                        well.AddLiquid(liquid, remainingLiquidVolumeToAssign);
                        remainingLiquidVolumeToAssign -= availableVolume;
                        if (remainingLiquidVolumeToAssign <= 0)
                            break;
                    }
                }

                if (remainingLiquidVolumeToAssign <= 0)
                    break;
            }

            if(liquidAssignmentFound)
            {
                return Result.Failure($"Unable to add liquid '{liquid.GetId()}' since no positions were assigned to the liquid");
            }

            return Result.Ok();
        }

        public Result<LiquidContainer> RequestLiquid(Liquid liquid, double desiredVolume)
        {
            var containers = _deck.GetLiquidContainers().Where(x => x.ContainsLiquid(liquid));
            foreach(var container in containers)
            {
                if(container.Volume > desiredVolume)
                {
                    return Result.Ok(container);
                }
            }
            
            return Result.Failure<LiquidContainer>($"Insufficient volume {desiredVolume}uL of liquid '{liquid.GetId()}'");
        }

        public Result ClearLiquidAtPosition(LabwareAddress address)
        {
            var plates = _deck.GetPlates();
            var targetPlate = plates.FirstOrDefault(x => x.PositionId == address.PositionId);
            if (targetPlate == null)
            {
                return Result.Failure($"No plate found in position {address.PositionId}");
            }

            var well = targetPlate.GetWell(address);
            if (well == null)
            {
                return Result.Failure($"No well found at address {address.ToAlphaAddress()}"); ;
            }

            well.Clear();
            return Result.Ok();
        }

    }
}