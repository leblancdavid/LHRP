namespace LHRP.Api.Runtime
{
    public interface IStateSnapshot<T>
    {
        T GetSnapshot();
    }
}
