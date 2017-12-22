using LHRP.Api.Common;
using LHRP.Api.Devices;

namespace LHRP.Api.Labware.Plates
{
    public class Well
    {
        public double WellCapacity { get; private set; }
        public double DeadVolume { get;private set; }
        public double CurrentVolume { get; private set; }

        public LabwareAddress Address { get; private set; }
        public Position RelativePosition { get; private set; }

        public Result Aspirate(double volume)
        {
            if(volume > CurrentVolume)
            {
                return Result.Fail("Insufficient volume in well '" + Address.ToString() + "'.");
            }
            CurrentVolume -= volume;
            return Result.Ok();
        }

        public Result Dispense(double volume)
        {
            if(volume + CurrentVolume > WellCapacity)
            {
                return Result.Fail("Dispensing " + volume + "uL into well '" + Address.ToString() + "' would exceed well capacity.");
            }
            CurrentVolume += volume;
            return Result.Ok();
        }
    }
}