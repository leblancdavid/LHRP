using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Instrument.NimbusLite.Devices.Pipettor;

namespace LHRP.Instrument.NimbusLite.Instrument
{
    public class NimbusLiteInstrument : IInstrument
    {
        private IPipettor _pipettor;
        public NimbusLiteInstrument()
        {
            _pipettor = new IndependentChannelPipettor();
        }

        public IDeck Deck { get; }

        public IDevice GetDevice(int id)
        {
        throw new System.NotImplementedException();
        }

        public IPipettor GetPipettor()
        {
            return _pipettor;
        }
    }
}