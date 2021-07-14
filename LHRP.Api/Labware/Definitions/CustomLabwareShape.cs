using LHRP.Api.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Labware.Definitions
{
    public abstract class CustomLabwareShape : ILabwareShape
    {
        protected List<ILabwareShape> _segments = new List<ILabwareShape>();
        public IEnumerable<ILabwareShape> Segments => _segments;

        public double ClearanceHeight => Origin.Z + Dimensions.Height;

        public double TotalVolume => _segments.Sum(x => x.TotalVolume);

        public Coordinates Origin { get; private set; }

        public Dimensions Dimensions => CalculateDimensions();

        public CustomLabwareShape(Coordinates origin)
        {
            Origin = origin;
        }

        public void AddSegment(ILabwareShape segment)
        {
            _segments.Add(segment);
        }

        public void insertSegmentAt(int index, ILabwareShape segment)
        {
            _segments.Insert(index, segment);
        }

        public void RemoveSegmentAt(int index)
        {
            _segments.RemoveAt(index);
        }

        protected Dimensions CalculateDimensions()
        {
            double minX = _segments.Min(x => x.Origin.X);
            if(minX > Origin.X)
            {
                minX = Origin.X;
            }
            double maxX = _segments.Max(x => x.Origin.X + x.Dimensions.Width);


            double minY = _segments.Min(x => x.Origin.Y);
            if (minY > Origin.Y)
            {
                minY = Origin.Y;
            }
            double maxY = _segments.Max(x => x.Origin.X + x.Dimensions.Length);

            double minZ = _segments.Min(x => x.Origin.Z);
            if (minY > Origin.Y)
            {
                minY = Origin.Y;
            }
            double maxZ = _segments.Max(x => x.Origin.X + x.Dimensions.Height);

            return new Dimensions(maxX - minX, maxY - minY, maxZ - minZ);
        }
    }
}
