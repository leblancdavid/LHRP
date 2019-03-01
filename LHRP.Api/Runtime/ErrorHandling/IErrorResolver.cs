namespace LHRP.Api.Runtime.ErrorHandling
{
    public interface IErrorResolver
    {
         void Resolve(IRuntimeCommandQueue queue);
    }
}