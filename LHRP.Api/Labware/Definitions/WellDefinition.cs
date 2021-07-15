namespace LHRP.Api.Labware
{
    public class WellDefinition
    {
        public double WellCapacity { get; private set; }
        public double DeadVolume { get; private set; }

        public WellDefinition(double wellCapacity = 0.0, double deadVolume = 0.0)
        {
            WellCapacity = wellCapacity;
            DeadVolume = deadVolume;
        }
    }
}