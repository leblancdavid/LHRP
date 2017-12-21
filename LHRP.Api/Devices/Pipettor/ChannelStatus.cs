namespace LHRP.Api.Devices.Pipettor
{
    public class ChannelStatus : IDeviceStatus
    {
        public bool HasTip { get; set; }
        public double TipCapacity { get; set; }
        public double CurrentVolume { get;  set; }

        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public Position CurrentPosition { get; set; }
    }
}