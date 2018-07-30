namespace LHRP.Api.Instrument.LiquidManagement
{
    public class LiquidManagerConfiguration
    {
        public bool AutoLiquidAssignment { get; private set; }

        public LiquidManagerConfiguration(bool autoAssignment)
        {
            AutoLiquidAssignment = autoAssignment;
        }
    }
}