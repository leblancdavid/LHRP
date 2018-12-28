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
        public Result RemoveLiquidFromPosition(int positionId, LabwareAddress address, double volume)
        {
            var plates = _deck.GetPlates();
            var targetPlate = plates.FirstOrDefault(x => x.PositionId == positionId);
            if(targetPlate == null)
            {
                return Result.Fail($"No plate found in position {positionId}");
            }
            
            var well = targetPlate.GetWell(address);
            if(well.IsFailure)
            {
                return well;
            }

            // if(well.Value.ContainsLiquid(target.Liquid))
            // {
            //     if(!_configuration.AutoLiquidAssignment)
            //     {
            //         return Result.Fail($"Liquid {target.Liquid.AssignedId} not found in well({well.Value.Address.Row},{well.Value.Address.Column})");
            //     }
                
            //     well.Value.AddLiquid(target.Liquid, target.Volume);
            // }

            if(well.Value.Volume < volume)
            {
                return Result.Fail($"Insufficient liquid found in well({well.Value.Address.Row},{well.Value.Address.Column})");
            }
            
            well.Value.Remove(volume);

            return Result.Ok();
        }

        public Result AddLiquidToPosition(int positionId, LabwareAddress address, Liquid liquidToAssign, double volume)
        {
             var plates = _deck.GetPlates();
            var targetPlate = plates.FirstOrDefault(x => x.PositionId == positionId);
            if(targetPlate == null)
            {
                return Result.Fail($"No plate found in position {positionId}");
            }
            
            var well = targetPlate.GetWell(address);
            if(well.IsFailure)
            {
                return well;
            }

            well.Value.AddLiquid(liquidToAssign, volume);

            return Result.Ok();
        }

        public Result<TransferTarget> RequestTargetLiquid(Liquid liquid, double desiredVolume)
        {
            throw new System.NotImplementedException();
        }
    }
}