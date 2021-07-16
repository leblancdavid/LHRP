namespace LHRP.Api.Labware
{
    public class WellDefinition
    {
        public ILabwareShape WellShape { get; private set; }

        public WellDefinition(ILabwareShape wellShape)
        {
            WellShape = wellShape;
        }
    }
}