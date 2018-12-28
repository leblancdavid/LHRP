namespace LHRP.Api.Runtime.Errors
{
    public interface IErrorResolver
    {
         void Resolve(IRuntimeCommandQueue queue);
    }
}