namespace LHRP.Api.Devices.Pipettor
{
    public class AspirateCommand : Command
    {
        public double Volume { get; set; }
        public Coordinates Position { get; set; }
        
    }
}