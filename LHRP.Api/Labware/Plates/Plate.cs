using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;

namespace LHRP.Api.Labware.Plates
{
    public class Plate : Labware
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
                foreach(var well in _wells.Values)
                {
                    well.AbsolutePosition.X = value.X - _absolutePosition.X;
                    well.AbsolutePosition.Y = value.Y - _absolutePosition.Y;
                    well.AbsolutePosition.Z = value.Z - _absolutePosition.Z;
                }
                _absolutePosition = value;
            }
        }
        
        private Dictionary<LabwareAddress, Well> _wells = new Dictionary<LabwareAddress, Well>();

        public Plate(PlateDefinition definition)
        {
            Definition = definition;
            InitializeWells(definition);
        }
        
        public override Result<Coordinates> GetRealCoordinates(LabwareAddress address)
        {
            if(!_wells.ContainsKey(address))
            {
                return Result.Fail<Coordinates>("Invalid plate address");
            }

            return Result.Ok(_wells[address].AbsolutePosition);
        }

        private void InitializeWells(PlateDefinition definition)
        {
            _wells.Clear();
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

                    var labwareAddress = new LabwareAddress(i + 1, j + 1);
                    
                    _wells.Add(labwareAddress, new Well(definition.WellDefinition));
                }
            }
        }

    }
}