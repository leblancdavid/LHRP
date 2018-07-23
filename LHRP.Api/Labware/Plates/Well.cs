
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;

namespace LHRP.Api.Labware.Plates
{
    public class Well
    {
        public WellDefinition Definition { get; private set; }
        public LabwareAddress Address { get; private set; }
        public Coordinates AbsolutePosition { get; private set; }

        public Well(WellDefinition definition)
        {
            Definition = definition;
        }

        // public Result Aspirate(double volume)
        // {
        //     if(volume > CurrentVolume)
        //     {
        //         return Result.Fail("Insufficient volume in well '" + Address.ToString() + "'.");
        //     }
        //     CurrentVolume -= volume;
        //     return Result.Ok();
        // }

        // public Result Dispense(double volume)
        // {
        //     if(volume + CurrentVolume > WellCapacity)
        //     {
        //         return Result.Fail("Dispensing " + volume + "uL into well '" + Address.ToString() + "' would exceed well capacity.");
        //     }
        //     CurrentVolume += volume;
        //     return Result.Ok();
        // }
    }
}