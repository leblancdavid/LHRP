﻿using LHRP.Api.Instrument;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Protocol.Transfers;

namespace LHRP.Api.Devices.Pipettor
{
    public class ChannelPipettingTransfer
    {
        public double Volume { get; private set; }
        public int Channel { get; private set; }
        public TransferType Transfer { get; private set; }
        public ILiquidHandlingParameters Parameters { get; private set; }
        public LiquidContainer Container { get; private set; }
        public Liquid? Liquid { get; private set; }

        //there will be more parameters obviously
        public ChannelPipettingTransfer(double volume, Liquid? liquid, ILiquidHandlingParameters parameters, int channel, LiquidContainer container, TransferType transferType)
        {
            Volume = volume;
            Channel = channel;
            Liquid = liquid;
            Parameters = parameters;
            Container = container;
            Transfer = transferType;
        }

        public Coordinates GetPipetteCoordinates()
        {
            var bottom = Container.GetAbsoluteBottomPosition();
            if (Parameters.PipettingPositionType == PipettePositionType.FromContainerBottom)
            {
                return new Coordinates(bottom.X, bottom.Y, bottom.Z + Parameters.PipettingHeight);
            }
            else
            {
                var liquidLevel = Container.GetLiquidLevelPosition();
                var pipetteCoordinates = new Coordinates(liquidLevel.X, liquidLevel.Y, liquidLevel.Z + Parameters.PipettingHeight);
                if (pipetteCoordinates.Z < bottom.Z)
                    pipetteCoordinates.Z = bottom.Z; //limit to bottom
                return pipetteCoordinates;
            }
        }

    }
}
