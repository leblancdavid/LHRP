using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Liquid
{
    public class LiquidContainer
    {
        public double Volume { get; private set; }
        private List<Liquid> _liquids = new List<Liquid>();
        public IEnumerable<Liquid> Liquids => _liquids;
        private Dictionary<string,double> _liquidVolumes = new Dictionary<string, double>();

        public void DispenseLiquid(Liquid liquid, double volume)
        {
            if(!ContainsLiquid(liquid))
            {
                _liquids.Add(liquid);
                _liquidVolumes[liquid.AssignedId] = 0.0;
            }
            _liquidVolumes[liquid.AssignedId] += volume;

        }

        public bool ContainsLiquid(Liquid liquid)
        {
            return _liquids.Any(l => l.AssignedId == liquid.AssignedId);
        }


    }
}