using LHRP.Api.Instrument;

namespace LHRP.Api.Labware
{
    public interface ILabwareShape
    {
        double ClearanceHeight { get; }
        double TotalVolume { get; }
        Coordinates Origin { get; }
        Coordinates Center { get; }
        Dimensions Dimensions { get; }

        double GetHeightAtVolume(double volume);
        
    }
}
