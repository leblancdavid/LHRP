using LHRP.Api.Devices.Pipettor;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public class TipPickupRuntimeError : RuntimeError
    {
        public int TipTypeId { get; private set; }
        public ChannelPattern ChannelErrors { get; private set; }
        public TipChannelPattern RequestedPattern { get; private set; }
        public TipPickupRuntimeError(string errorMessage, 
            int tipTypeId,
            ChannelPattern channelErrors,
            TipChannelPattern requestedPattern) 
            : base(errorMessage)
        {
            TipTypeId = tipTypeId;
            ChannelErrors = channelErrors;
            RequestedPattern = requestedPattern;
        }
    }
}
