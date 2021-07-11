using LHRP.Api.Labware;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public class InsufficientSourceVolumeRuntimeError : RuntimeError
    {
        public double VolumeRequested { get; private set; }
        public double VolumeInContainer { get; private set; }
        public LabwareAddress Address { get; private set; }

        public InsufficientSourceVolumeRuntimeError(string errorMessage, 
            LabwareAddress address, double volumeInContainer, double volumeRequested) : base(errorMessage)
        {
            Address = address;
            VolumeInContainer = volumeInContainer;
            VolumeRequested = volumeRequested;
        }
    }
}
