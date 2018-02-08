namespace LHRP.Api.Protocol.TransferPattern
{
    public class Transfer
    {
        public Transfer(TransferTarget source, TransferTarget destination)
        {
            Source = source;
            Destination = destination;
        }

        public TransferTarget Source { get; private set; }
        public TransferTarget Destination { get; private set; }   
    }
}