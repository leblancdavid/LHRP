namespace LHRP.Api.Runtime
{
    public interface IStateSnapshotGetter<T>
    {
        T GetSnapshot();
    }
}
