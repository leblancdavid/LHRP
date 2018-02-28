using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Protocol.Pipetting
{
    public class AspirateOptions
    {
        public TransferGroup TransferGroup { get; private set; }
        public double Volume { get; private set; }

        //Other properties here!!!
        public AspirateOptions(TransferGroup group, double volume)
        {
            TransferGroup = group;
            Volume = volume;
        }
    }
}