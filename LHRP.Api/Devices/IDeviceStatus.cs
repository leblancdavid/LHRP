namespace LHRP.Api.Devices
{
    public interface IDeviceStatus
    {
        bool HasError { get; set; }
        string ErrorMessage { get; set; }
        Position CurrentPosition { get; set; }
    }
}