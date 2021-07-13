using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Liquids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Labware
{
    public abstract class LiquidContainingLabware : Labware
    {

        protected Dictionary<LabwareAddress, LiquidContainer> _containers = new Dictionary<LabwareAddress, LiquidContainer>();

        public IEnumerable<LiquidContainer> GetContainers() => _containers.Values;
        public IEnumerable<LiquidContainer> GetLiquidContainers(Liquid withLiquid)
        {
            return _containers.Values.Where(x => x.Liquid != null && x.Liquid.Match(withLiquid));
        }

        public override Coordinates? GetRealCoordinates(LabwareAddress address)
        {
            if (!_containers.ContainsKey(address))
            {
                return null;
            }

            return _containers[address].AbsolutePosition;
        }

        public virtual LiquidContainer? GetContainer(LabwareAddress address)
        {
            if (!_containers.ContainsKey(address))
            {
                return null;
            }

            return _containers[address];
        }
    }
}
