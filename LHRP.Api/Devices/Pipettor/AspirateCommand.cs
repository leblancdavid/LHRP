namespace LHRP.Api.Devices.Pipettor
{
    public class AspirateCommand : Command
    {
        public double Volume { get; set; }
        public Position Position { get; set; }
        
    }
}