namespace LHRP.Api.Protocol.Transfers.OneToOne
{
    public class OneToOneTransfer : Transfer
    {
        public TransferTarget Source {get; private set;}
        public OneToOneTransfer(TransferTarget source, TransferTarget destination) : base(destination)
        {
            Source = source;
        }
    }
}