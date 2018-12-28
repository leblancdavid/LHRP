using CSharpFunctionalExtensions;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Instrument.LiquidManagement
{
    public interface ILiquidManager
    {
        Result AddLiquidToPosition(int positionId, LabwareAddress address, Liquid liquidToAssign, double volume);
        Result<TransferTarget> RequestTargetLiquid(Liquid liquid, double desiredVolume);
        Result RemoveLiquidFromPosition(int positionId, LabwareAddress address, double volume);
    }
}