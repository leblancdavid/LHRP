using LHRP.Api.Common;

namespace LHRP.Api.Labware
{
    public interface IConsumableLabware
    {
        bool CanConsume(LabwareAddress address, double ammount);
        Result Consume(LabwareAddress address, double ammount);
        Result ConsumeNextAvailable(double ammount);
    }
}