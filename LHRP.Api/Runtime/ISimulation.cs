namespace LHRP.Api.Runtime
{
    public interface ISimulation
    {
        uint SimulationSpeedFactor { get; set; }
        double FailureRate { get; set; }
    }
}