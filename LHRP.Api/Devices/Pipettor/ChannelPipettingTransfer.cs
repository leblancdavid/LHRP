using LHRP.Api.Instrument;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Devices.Pipettor
{
    public class ChannelPipettingTransfer
    {
        public Liquid Liquid { get; private set; }
        public Coordinates PipetteLocation { get; private set; }
        public LabwareAddress Address { get; private set; }
        public double Volume { get; private set; }
        public int Channel { get; private set; }
        public TransferType Transfer { get; private set; }


        //there will be more parameters obviously
        public ChannelPipettingTransfer(double volume, int channel, Liquid liquid, Coordinates location, LabwareAddress address, TransferType transferType)
        {
            Volume = volume;
            Channel = channel;
            Liquid = liquid;
            PipetteLocation = location;
            Address = address;
            Transfer = transferType;
        }

    }
}
