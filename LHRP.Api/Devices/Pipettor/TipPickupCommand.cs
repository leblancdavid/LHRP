namespace LHRP.Api.Devices.Pipettor
{
    public class TipPickupCommand
    {
        public string ChannelPattern { get; set; }
        public Coordinates Position { get; set; }
    }
}