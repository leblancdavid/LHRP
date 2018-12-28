namespace LHRP.Api.Runtime.Errors
{
    public class DefaultErrorHandler : IErrorHandler
    {
        public IErrorResolver HandleError(RuntimeError error)
        {
            throw new System.NotImplementedException();
        }
    }
}