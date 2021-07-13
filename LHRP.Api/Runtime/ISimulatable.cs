namespace LHRP.Api.Runtime
{
    public interface ISimulatable<T>
    {
        uint SimulationSpeedFactor { get; set; }
        double FailureRate { get; set; }

        T GetSimulation();
    }
}