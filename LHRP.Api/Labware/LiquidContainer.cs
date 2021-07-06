using LHRP.Api.CoordinateSystem;
using LHRP.Api.Liquids;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Labware
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

        public double MaxVolume { get; protected set; }

        public double AvailableVolume
        {
            get
            {
                return MaxVolume - Volume;
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

        public bool IsAssigned
        {
            get
            {
                return _liquids.Count > 0;
            }
        }
        public Coordinates AbsolutePosition { get; protected set; }
        public LabwareAddress Address { get; protected set; }

        public LiquidContainer(LabwareAddress address, Coordinates absolutePosition, double maxVolume)
        {
            Address = address;
            AbsolutePosition = absolutePosition;
            MaxVolume = maxVolume;
        }


        public void AddLiquid(Liquid liquid, double volume)
        {
            if(!ContainsLiquid(liquid))
            {
                _liquids.Add(liquid);
                _liquidVolumes[liquid.AssignedId] = 0.0;
            }

            if(_liquidVolumes[liquid.AssignedId] + volume > MaxVolume)
            {
                _liquidVolumes[liquid.AssignedId] = MaxVolume;
            }
            else
            {
                _liquidVolumes[liquid.AssignedId] += volume;
            }
        }

        public void AssignLiquid(Liquid liquid)
        {
            _liquids.Clear();
            _liquidVolumes.Clear();
            _liquids.Add(liquid);
            _liquidVolumes[liquid.AssignedId] = 0.0;
        }

        public void Remove(double volume)
        {
            var totalVolume = Volume;
            if(volume > totalVolume)
            {
                volume = totalVolume;
            }
            
            var volumeFactor = 1.0 - volume/totalVolume;
            foreach(var liquid in _liquidVolumes.Keys.ToList())
            {
                _liquidVolumes[liquid] *= volumeFactor;
            }
        }

        public bool ContainsLiquid(Liquid liquid)
        {
            return _liquids.Any(l => l.AssignedId == liquid.AssignedId);
        }

        public void Clear()
        {
            _liquids.Clear();
            _liquidVolumes.Clear();
        }


    }
}