namespace LHRP.Api.Labware
{
    public interface ILabwareDefinition
    {
        int Id { get; }
        string DisplayName { get; }
        ILabwareShape Shape { get; }

        Labware CreateInstance(int instanceId);

    }
}
