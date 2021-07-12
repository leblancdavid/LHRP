namespace LHRP.Api.Runtime.Compilation
{
    public interface ICompilable
    {
        ProcessResult Compile(IRuntimeEngine engine);
    }
}
