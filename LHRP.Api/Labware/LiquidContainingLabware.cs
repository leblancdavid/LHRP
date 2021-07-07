using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Liquids;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Labware
{
    public class LiquidContainingLabware : Labware
    {

        protected Dictionary<LabwareAddress, LiquidContainer> _containers = new Dictionary<LabwareAddress, LiquidContainer>();

        public IEnumerable<LiquidContainer> GetContainers() => _containers.Values;
        public override Result<Coordinates> GetRealCoordinates(LabwareAddress address)
        {
            if (!_containers.ContainsKey(address))
            {
                return Result.Failure<Coordinates>("Invalid plate address");
            }

            return Result.Ok(_containers[address].AbsolutePosition);
        }
    }
}
