using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;

namespace LHRP.Api.Runtime.Resources
{
    public interface IResourceInitializer
    {
        ProcessResult Initialize(IInstrument instrument, ResourcesUsage resources);
    }
}
