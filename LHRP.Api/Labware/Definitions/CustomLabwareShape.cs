using LHRP.Api.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Labware.Definitions
{
    public class CustomLabwareShape : ILabwareShape
    {
        protected List<ILabwareShape> _segments = new List<ILabwareShape>();
        public IEnumerable<ILabwareShape> Segments => _segments;

        public double ClearanceHeight => Origin.Z + Dimensions.Height;

        public double TotalVolume => _segments.Sum(x => x.TotalVolume);

        public Coordinates Origin { get; private set; }

        public Dimensions Dimensions => CalculateDimensions();

        public Coordinates Center
        {
            get
            {
                var dimensions = Dimensions;
                return new Coordinates(Origin.X + dimensions.Width / 2.0,
                    Origin.Y + dimensions.Length / 2.0,
                    Origin.Z + dimensions.Height / 2.0);
            }
        }

        public CustomLabwareShape(Coordinates origin)
        {
            Origin = origin;
        }

        public void AddSegment(ILabwareShape segment)
        {
            _segments.Add(segment);
            //
            _segments = _segments.OrderBy(x => x.Origin.Z).ToList();
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
            double maxZ = _segments.Max(x => x.Origin.Z + x.Dimensions.Height);

            return new Dimensions(maxX - minX, maxY - minY, maxZ - minZ);
        }

        public double GetHeightAtVolume(double volume)
        {
            if(!_segments.Any())
            {
                return 0.0;
            }
               
            double remainingVolume = volume;
            int i;
            for (i = 0; i < _segments.Count - 1; ++i)
            {
                if(remainingVolume < _segments[i].TotalVolume)
                {
                    break;
                }
                remainingVolume -= _segments[i].TotalVolume;
            }

            return _segments[i].GetHeightAtVolume(remainingVolume) + Origin.Z;
        }
    }
}
