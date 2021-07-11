using CSharpFunctionalExtensions;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Runtime;

namespace LHRP.Api.Instrument
{
    public interface ILiquidManager : IStateSnapshot<ILiquidManager>
    {
        Result AddLiquidToPosition(LabwareAddress address, Liquid liquidToAssign, double volume);
        Result AddLiquid(Liquid liquid, double volume);
        Result<LiquidContainer> RequestLiquid(Liquid liquid, double desiredVolume);
        Result RemoveLiquidFromPosition(LabwareAddress address, double volume);
        Result ClearLiquidAtPosition(LabwareAddress address);
    }
}