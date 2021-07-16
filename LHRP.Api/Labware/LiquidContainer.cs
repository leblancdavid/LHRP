using LHRP.Api.Instrument;
using LHRP.Api.Liquids;
using LHRP.Api.Runtime;

namespace LHRP.Api.Labware
{
    public class LiquidContainer : ISnapshotCreator<LiquidContainer>
    {
        public double Volume { get; protected set; }

        public double MaxVolume 
        { 
            get
            {
                return ContainerShape.TotalVolume;
            }
        }

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
        
        public bool IsEmpty
        {
            get
            {
                return _liquid == null || Volume == 0.0;
            }
        }


        public Coordinates AbsolutePosition { get; protected set; }
        public LabwareAddress Address { get; protected set; }
        public ILabwareShape ContainerShape { get; protected set; }

        public LiquidContainer(LabwareAddress address, Coordinates absolutePosition, ILabwareShape containerShape)
        {
            Address = address;
            AbsolutePosition = absolutePosition;
            ContainerShape = containerShape;
        }


        public void AddLiquid(Liquid liquid, double volume)
        {
            if (volume < 0.0)
                return;

            Volume += volume;
            if(Volume > MaxVolume)
            {
                Volume = MaxVolume;
            }

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

        public Coordinates GetAbsoluteBottomPosition()
        {
            var center = ContainerShape.Center;
            return new Coordinates(AbsolutePosition.X + center.X,
                AbsolutePosition.Y + center.Y,
                AbsolutePosition.Z + ContainerShape.Origin.Z);
        }

        public Coordinates GetLiquidLevelPosition()
        {
            var center = ContainerShape.Center;
            var liquidHeight = ContainerShape.GetHeightAtVolume(Volume);
            return new Coordinates(AbsolutePosition.X + center.X,
                AbsolutePosition.Y + center.Y,
                AbsolutePosition.Z + liquidHeight);
        }

        public virtual LiquidContainer CreateSnapshot()
        {
            var container = new LiquidContainer(Address, AbsolutePosition, ContainerShape);
            if(IsAssigned)
            {
                container.AssignLiquid(this.Liquid!);
            }
            if(Volume > 0.0)
            {
                container.AddLiquid(this.Liquid!, Volume);
            }
            return container;
        }
    }
}