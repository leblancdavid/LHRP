using LHRP.Api.Instrument;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Labware.Definitions
{
    public interface ILabwareShape
    {
        double ClearanceHeight { get; }
        double TotalVolume { get; }
        Coordinates Origin { get; }
        Dimensions Dimensions { get; }
        
    }
}
