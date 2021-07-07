using System;

namespace LHRP.Api.Liquids
{
    public class Liquid
    {
        public Guid UniqueId { get; private set; }
        private string _assignedId;
        public string AssignedId 
        { 
            get
            {
                if(string.IsNullOrEmpty(_assignedId))
                    return UniqueId.ToString();
                return _assignedId;
            }
        }
        public LiquidType LiquidType { get; private set; }
        public bool IsUnknown { get; private set; }

        public Liquid(LiquidType type = LiquidType.Default, bool isUnknown = true)
        {
            LiquidType = type;
            IsUnknown = isUnknown;
            _assignedId = "";
            UniqueId = Guid.NewGuid();
        }
    }
}