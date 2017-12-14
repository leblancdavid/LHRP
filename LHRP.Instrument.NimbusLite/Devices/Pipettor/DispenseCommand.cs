using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Devices.Pipettor
{
    public class DispenseCommand : Command
    {
        public DispenseCommand(double volume,
            int position) : base("Dispense")
        {
            SetValue("Volume", volume);
            SetValue("Position", position);
        }
    }
}