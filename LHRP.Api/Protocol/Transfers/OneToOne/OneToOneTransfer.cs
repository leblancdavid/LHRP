namespace LHRP.Api.Protocol.Transfers.OneToOne
{
    public class OneToOneTransfer : ITransfer
    {
        public TransferTarget Source { get; private set; }
        public TransferTarget Target { get; private set; }
        public OneToOneTransfer(TransferTarget source, TransferTarget target)
        {
            Source = source;
            Target = target;
        }
    }
}