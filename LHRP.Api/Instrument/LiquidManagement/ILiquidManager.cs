using CSharpFunctionalExtensions;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Instrument.LiquidManagement
{
    public interface ILiquidManager
    {
        Result<TransferTarget> RequestTargetLiquid(Liquid liquid, double desiredVolume);
        Result AspirateLiquidFrom(TransferTarget target);
        Result DispenseLiquidTo(TransferTarget target);
    }
}