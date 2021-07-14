using LHRP.Api.Instrument;
using System.Collections.Generic;

namespace LHRP.Api.Devices
{
    public interface IDeviceStatus
    {
        bool HasErrors { get; }
        IEnumerable<string> ErrorMessages { get; }
        Coordinates? CurrentPosition { get; set; }
    }
}