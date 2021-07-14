namespace LHRP.Api.Runtime
{
    public interface ISnapshotCreator<T>
    {
        T CreateSnapshot();
    }
}
