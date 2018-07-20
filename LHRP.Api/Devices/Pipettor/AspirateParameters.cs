using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Devices.Pipettor
{
    public class AspirateParameters
    {
        public TransferGroup TransferGroup { get; private set; }
        public double Volume { get; private set; }
        public  AspirateParameters(TransferGroup transferGroup,
            double volume)
        {
            TransferGroup = transferGroup;
            Volume = volume;
        }

    }
}