using CSharpFunctionalExtensions;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Instrument.LiquidManagement
{
    public interface ILiquidManager
    {
        Result<LiquidTransferTarget> RequestTargetLiquid(Liquid liquid, double desiredVolume);
        Result AspirateLiquidFrom(LiquidTransferTarget target);
        Result DispenseLiquidTo(LiquidTransferTarget target);
    }
}