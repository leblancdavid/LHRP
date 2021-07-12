using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;

namespace LHRP.Api.Runtime.Compilation
{
    public interface ICompiler
    {
        Result Compile(IRunnable runnable, IRuntimeEngine engine);
    }
}
