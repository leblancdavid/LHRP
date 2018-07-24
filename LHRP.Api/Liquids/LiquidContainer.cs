using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Liquids
{
    public class LiquidContainer
    {
        public double Volume 
        { 
            get
            {
                return _liquidVolumes.Values.Sum();
            }
        }
        private List<Liquid> _liquids = new List<Liquid>();
        public IEnumerable<Liquid> Liquids => _liquids;
        private Dictionary<string,double> _liquidVolumes = new Dictionary<string, double>();

        public bool IsPure
        {
            get
            {
                return _liquids.Count <= 1;
            }
        }

        public void AddLiquid(Liquid liquid, double volume)
        {
            if(!ContainsLiquid(liquid))
            {
                _liquids.Add(liquid);
                _liquidVolumes[liquid.AssignedId] = 0.0;
            }
            _liquidVolumes[liquid.AssignedId] += volume;

        }

        public void Remove(double volume)
        {
            var totalVolume = Volume;
            if(volume > totalVolume)
            {
                volume = totalVolume;
            }
            
            var volumeFactor = 1.0 - volume/totalVolume;
            foreach(var liquid in _liquidVolumes.Keys)
            {
                _liquidVolumes[liquid] *= volumeFactor;
            }
        }

        public bool ContainsLiquid(Liquid liquid)
        {
            return _liquids.Any(l => l.AssignedId == liquid.AssignedId);
        }


    }
}