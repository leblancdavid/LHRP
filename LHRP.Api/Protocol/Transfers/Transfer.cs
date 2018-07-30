namespace LHRP.Api.Protocol.Transfers
{
    public class Transfer
    {
        public Transfer(TransferTarget target)
        {
            Target = target;
        }

        public TransferTarget Target { get; private set; }   
    }
}