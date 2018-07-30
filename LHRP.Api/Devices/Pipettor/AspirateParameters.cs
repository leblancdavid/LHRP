using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Devices.Pipettor
{
    public class AspirateParameters
    {
        public TransferGroup TransferGroup { get; private set; }
        public  AspirateParameters(TransferGroup transferGroup)
        {
            TransferGroup = transferGroup;
        }

    }
}