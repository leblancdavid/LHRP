using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Instrument.LiquidManagement
{
    public class LiquidManager : ILiquidManager
    {
        private IDeck _deck;
        public LiquidManager(IDeck deck)
        {
            _deck = deck;
        }
        public Result AspirateLiquidFrom(TransferTarget target)
        {
            var plates = _deck.GetPlates();
            var targetPlate = plates.FirstOrDefault(x => x.PositionId == target.PositionId);
            if(targetPlate == null)
            {
                return Result.Fail($"No plate found in position {target.PositionId}");
            }
            
            var well = targetPlate.GetWell(target.Address);
            if(well.IsFailure)
            {
                return well;
            }

            if(well.Value.ContainsLiquid(target.Liquid))
            {
                return Result.Fail($"Liquid {target.Liquid.AssignedId} not found in well({well.Value.Address.Row},{well.Value.Address.Column})");
            }

            if(well.Value.Volume < target.Volume)
            {
                return Result.Fail($"Insufficient liquid {target.Liquid.AssignedId} found in well({well.Value.Address.Row},{well.Value.Address.Column})");
            }

            well.Value.Remove(target.Volume);

            return Result.Ok();
        }

        public Result DispenseLiquidTo(TransferTarget target)
        {
            var plates = _deck.GetPlates();
            var targetPlate = plates.FirstOrDefault(x => x.PositionId == target.PositionId);
            if(targetPlate == null)
            {
                return Result.Fail($"No plate found in position {target.PositionId}");
            }
            
            var well = targetPlate.GetWell(target.Address);
            if(well.IsFailure)
            {
                return well;
            }

            well.Value.AddLiquid(target.Liquid, target.Volume);

            return Result.Ok();
        }

        public Result<TransferTarget> RequestTargetLiquid(Liquid liquid, double desiredVolume)
        {
            throw new System.NotImplementedException();
        }
    }
}