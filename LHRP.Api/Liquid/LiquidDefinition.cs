namespace LHRP.Api.Liquid
{
    public class LiquidDefinition
    {
        public LiquidType LiquidType { get; private set; }
        //TODO Define liquid parameters later
        public double Value { get; private set; }

        public LiquidDefinition(LiquidType liquidType)
        {
            LiquidType = liquidType;
        }
    }
}