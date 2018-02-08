namespace LHRP.Api.Devices
{
    public interface IDeviceStatus
    {
        bool HasError { get; set; }
        string ErrorMessage { get; set; }
        Coordinates CurrentPosition { get; set; }
    }
}