using LHRP.Api.CoordinateSystem;
using LHRP.Api.Instrument;
using LHRP.Domain.Tests.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Domain.Tests.Instrument
{
    public static class InstrumentDataProvider
    {
        public static IDeck GetTestDeck(int numPositions)
        {
            var positions = new List<DeckPosition>();
            for(int i = 0; i < numPositions; ++i)
            {
                positions.Add(new DeckPosition(i + 1, new Coordinates(), new Coordinates()));
            }
            return new Deck(positions);
        }

        public static IInstrument GetSimulatedInstrument(int numPositions)
        {
            return new BaseInstrument(DeviceDataProvider.GetSimulatedIndependentChannelPipettor(), GetTestDeck(numPositions));
        }
    }
}
