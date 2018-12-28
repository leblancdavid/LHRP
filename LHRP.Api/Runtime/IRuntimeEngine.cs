using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Protocol;

namespace LHRP.Api.Runtime
{
    public interface IRuntimeEngine
    {
        IInstrument Instrument { get; }
        IRuntimeCommandQueue Commands { get; }

        void Pause();
        void Resume();
        void Start();
    }
}