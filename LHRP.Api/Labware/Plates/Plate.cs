using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Liquids;

namespace LHRP.Api.Labware.Plates
{
    public class Plate : LiquidContainingLabware<Well>
    {
        public int NumWells 
        {
            get
            {
                return Definition.Rows * Definition.Columns;
            }
        }
        public PlateDefinition Definition { get; private set; }

        public override Coordinates AbsolutePosition 
        { 
            get
            {
                return _absolutePosition;
            } 
            set
            {
                //Before we update the absolute position, move all the tips along with the rack.
                foreach(var well in _containers.Values)
                {
                    well.AbsolutePosition.X = value.X - _absolutePosition.X;
                    well.AbsolutePosition.Y = value.Y - _absolutePosition.Y;
                    well.AbsolutePosition.Z = value.Z - _absolutePosition.Z;
                }
                _absolutePosition = value;
            }
        }

         public override int PositionId
        {
            get
            {
                return _positionId;
            }
            protected set
            {
                _positionId = value;
                foreach (var well in _containers)
                {
                    well.Key.PositionId = value;
                    well.Value.Address.PositionId = value;
                }
            }
        }

        public Plate(PlateDefinition definition)
        {
            Definition = definition;
            InitializeWells(definition);
        }

        public Result<Well> GetWell(LabwareAddress address)
        {
            if(!_containers.ContainsKey(address))
            {
                // return Result.Failure<Well>("Sorry! Hope you feel better soon!!!");
                return Result.Failure<Well>("Invalid plate address");
            }

            return Result.Ok(_containers[address]);
        }

        public IEnumerable<Well> GetWellsWithLiquid(Liquid liquid)
        {
            return _containers.Values.Where(w => w.ContainsLiquid(liquid));
        }

        private void InitializeWells(PlateDefinition definition)
        {
            _containers.Clear();
            for(int i = 0; i < definition.Rows; ++i)
            {
                for(int j = 0; j < definition.Columns; ++j)
                {
                    var absolutePosition = new Coordinates()
                    {
                        X = AbsolutePosition.X + Definition.Offset.X + Definition.Spacing * j,
                        Y = AbsolutePosition.Y + Definition.Offset.Y + Definition.Spacing * i,
                        Z = AbsolutePosition.Z + Definition.Offset.Z
                    };

                    var labwareAddress = new LabwareAddress(i + 1, j + 1, _positionId);

                    _containers.Add(labwareAddress, new Well(labwareAddress, absolutePosition, definition.WellDefinition));
                }
            }
        }

    }
}