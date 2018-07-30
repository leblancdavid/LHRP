using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Devices.Pipettor
{
    public class AspirateParameters
    {
        public TransferGroup<Transfer> TransferGroup { get; private set; }
        public  AspirateParameters(TransferGroup<Transfer> transferGroup)
        {
            TransferGroup = transferGroup;
        }

    }
}