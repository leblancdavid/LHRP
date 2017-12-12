using LHRP.Api.Instrument;

namespace LHRP.Api.Protocol
{
    public interface IStep
    {
         void Run(IInstrument instrument);
    }
}