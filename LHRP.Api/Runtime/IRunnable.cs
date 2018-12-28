using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Runtime
{
    public interface IRunnable
    {
         Process Run(IRuntimeEngine enigne);
    }
}