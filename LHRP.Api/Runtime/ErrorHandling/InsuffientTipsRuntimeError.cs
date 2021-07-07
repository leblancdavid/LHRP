namespace LHRP.Api.Runtime.ErrorHandling
{
    public class InsuffientTipsRuntimeError : RuntimeError
    {
        public int TipTypeId { get; private set; }
        public InsuffientTipsRuntimeError(string errorMessage, int tipTypeId) : base(errorMessage)
        {
            TipTypeId = tipTypeId;
        }
    }
}