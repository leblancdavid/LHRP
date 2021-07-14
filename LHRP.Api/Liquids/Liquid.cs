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

        public bool Match(Liquid liquidPart)
        {
            if (liquidPart.GetId() == GetId())
            {
                return true;
            }
            return false;
        }

        public virtual bool ContainsLiquid(Liquid liquid)
        {
            return Match(liquid);
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
            if(Match(liquid))
            {
                var sameLiquid = new HeterogeneousLiquid();
                sameLiquid.Mix(this, 1.0);
                return sameLiquid;
            }

            if(ratio > 1.0)
            {
                ratio = 1.0;
            }

            if(liquid is HeterogeneousLiquid)
            {
                var hLiquid = liquid as HeterogeneousLiquid;
                return hLiquid!.Mix(this, 1.0 - ratio);
            }

            var outputLiquid = new HeterogeneousLiquid();
            outputLiquid.Mix(this, 1.0);
            outputLiquid.Mix(liquid, ratio);

            return outputLiquid;
        }

        
    }
}