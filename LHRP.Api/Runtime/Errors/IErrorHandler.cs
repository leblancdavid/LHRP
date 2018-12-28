namespace LHRP.Api.Runtime.Errors
{
    public interface IErrorHandler
    {
        IErrorResolver HandleError(RuntimeError error);         
    }
}