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
        throw new System.NotImplementedException();
        }

        public Result DispenseLiquidTo(TransferTarget target)
        {
        throw new System.NotImplementedException();
        }

        public Result<TransferTarget> GetTargetLiquid(Liquid liquid)
        {
        throw new System.NotImplementedException();
        }
    }
}