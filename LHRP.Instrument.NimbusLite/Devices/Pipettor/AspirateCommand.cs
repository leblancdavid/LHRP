using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Devices.Pipettor
{
    public class AspirateCommand : Command
    {
        public AspirateCommand(double volume,
            int position) : base("Aspirate")
        {
            SetValue("Volume", volume);
            SetValue("Position", position);
        }
    }
}