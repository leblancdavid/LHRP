using System.Collections.Generic;
using LHRP.Api.CoordinateSystem;

namespace LHRP.Api.Devices
{
    public interface IDeviceStatus
    {
        bool HasErrors { get; }
        IEnumerable<string> ErrorMessages { get; }
        Coordinates? CurrentPosition { get; set; }
    }
}