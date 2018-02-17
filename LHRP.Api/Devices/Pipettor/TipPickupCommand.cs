namespace LHRP.Api.Devices.Pipettor
{
    public class TipPickupCommand
    {
        public ChannelPattern ChannelPattern { get; set; }
        public Coordinates Position { get; set; }
    }
}