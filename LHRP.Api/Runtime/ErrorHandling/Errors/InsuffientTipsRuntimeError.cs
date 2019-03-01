namespace LHRP.Api.Runtime.ErrorHandling.Errors
{
    public class InsuffientTipsRuntimeError : RuntimeError
    {
        public int TipTypeId { get; private set; }
        public InsuffientTipsRuntimeError(int tipTypeId) : base($"Insufficient tips")
        {
            TipTypeId = tipTypeId;
        }
    }
}