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
        }
        
        public override Result<Coordinates> GetRealCoordinates(LabwareAddress address)
        {
            if(!_wells.ContainsKey(address))
            {
                return Result.Fail<Coordinates>("Invalid plate address");
            }

            return Result.Ok(_wells[address].AbsolutePosition);
        }

    }
}