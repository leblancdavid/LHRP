namespace LHRP.Api.Devices.Pipettor
{
    public class PipettorStatus : IDeviceStatus
    {
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public Position CurrentPosition { get; set; }
    }
}