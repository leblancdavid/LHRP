using System;

namespace LHRP.Api.Liquids
{
    public class Liquid
    {
        public Guid UniqueId { get; private set; }
        protected string _assignedId;

        public virtual string GetId() 
        { 
            return _assignedId;
        }

        public LiquidType LiquidType { get; private set; }
        public bool IsUnknown { get; private set; }

        public Liquid(LiquidType type = LiquidType.Default, bool isUnknown = true)
        {
            LiquidType = type;
            IsUnknown = isUnknown;
            UniqueId = Guid.NewGuid();
            _assignedId = UniqueId.ToString();
        }

        public Liquid(string name, LiquidType type = LiquidType.Default, bool isUnknown = true)
        {
            LiquidType = type;
            IsUnknown = isUnknown;
            UniqueId = Guid.NewGuid();
            _assignedId = name;
        }

        public virtual HeterogeneousLiquid Mix(Liquid liquid, double ratio)
        {
            return new HeterogeneousLiquid();
        }
    }
}