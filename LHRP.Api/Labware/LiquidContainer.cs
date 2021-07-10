using LHRP.Api.CoordinateSystem;
using LHRP.Api.Liquids;

namespace LHRP.Api.Labware
{
    public class LiquidContainer
    {
        public double Volume { get; protected set; }

        public double MaxVolume { get; protected set; }

        public double AvailableVolume
        {
            get
            {
                return MaxVolume - Volume;
            }
        }

        protected HeterogeneousLiquid? _liquid = null;
        public HeterogeneousLiquid? Liquid => _liquid;

        public bool IsPure
        {
            get
            {
                return _liquid == null || _liquid.IsPure();
            }
        }

        public bool IsAssigned
        {
            get
            {
                return _liquid != null;
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
            if(_liquid == null)
            {
                _liquid = new HeterogeneousLiquid(liquid);
            }
            else
            {
                _liquid.Mix(liquid, volume / (volume + Volume));
            }
            
        }

        public void AssignLiquid(Liquid liquid)
        {
            _liquid = new HeterogeneousLiquid(liquid);
        }

        public void Remove(double volume)
        {
            var totalVolume = Volume;
            if(volume > totalVolume)
            {
                Volume = 0.0;
            }
            else
            {
                Volume -= volume;
            }
        }

        public bool ContainsLiquid(Liquid liquid, bool exactMatch = false)
        {
            if(_liquid == null)
            {
                return false;
            }

            if(exactMatch)
            {
                return _liquid.Match(liquid);
            }

            return _liquid.ContainsLiquid(liquid);
        }

        public void Clear()
        {
            _liquid = null;
            Volume = 0.0;
        }


    }
}