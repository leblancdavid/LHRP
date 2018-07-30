using CSharpFunctionalExtensions;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Instrument.LiquidManagement
{
    public interface ILiquidManager
    {
        Result<TransferTarget> GetTargetLiquid(Liquid liquid);
        Result AspirateLiquidFrom(TransferTarget target);
        Result DispenseLiquidTo(TransferTarget target);
    }
}