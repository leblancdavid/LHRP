using LHRP.Api.Common;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;

namespace LHRP.Api.Labware.Plates
{
    public class Plate : Labware
    {
        public int NumWells { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        
        public override Result<Coordinates> GetRealCoordinates(LabwareAddress address)
        {
            return Result<Coordinates>.Fail("NOT IMPLEMENTED!!!");
        }

    }
}