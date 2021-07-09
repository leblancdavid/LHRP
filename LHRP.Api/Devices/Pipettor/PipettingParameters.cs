using LHRP.Api.CoordinateSystem;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;

namespace LHRP.Api.Devices.Pipettor
{
    public class PipettingParameters
    {
        public Liquid Liquid { get; private set; }
        public Coordinates PipetteLocation { get; private set; }
        public LabwareAddress Address { get; private set; }
        public double Volume { get; private set; }
        public int Channel { get; private set; }

        //there will be more parameters obviously
        public PipettingParameters(double volume, int channel, Liquid liquid, Coordinates location, LabwareAddress address)
        {
            Volume = volume;
            Channel = channel;
            Liquid = liquid;
            PipetteLocation = location;
            Address = address;
        }

    }
}
