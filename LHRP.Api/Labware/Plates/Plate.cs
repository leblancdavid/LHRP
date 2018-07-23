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
        public Plate(PlateDefinition definition)
        {
            Definition = definition;
        }
        
        public override Result<Coordinates> GetRealCoordinates(LabwareAddress address)
        {
            return Result.Fail<Coordinates>("NOT IMPLEMENTED!!!");
        }

    }
}