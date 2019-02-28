using CSharpFunctionalExtensions;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Instrument.LiquidManagement
{
    public interface ILiquidManager
    {
        Result AddLiquidToPosition(LabwareAddress address, Liquid liquidToAssign, double volume);
        Result<TransferTarget> RequestTargetLiquid(Liquid liquid, double desiredVolume);
        Result RemoveLiquidFromPosition(LabwareAddress address, double volume);
    }
}