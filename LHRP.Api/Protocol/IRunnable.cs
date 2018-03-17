using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol
{
    public interface IRunnable
    {
         Process Run(IInstrument instrument);
    }
}