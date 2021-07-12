using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime.Resources;
using LHRP.Api.Runtime.Scheduling;
using LHRP.Instrument.SimplePipettor.Devices.Pipettor;

namespace LHRP.Instrument.SimplePipettor.Instrument
{
    public class SimplePipettorInstrument : Api.Instrument.Instrument, IInstrument
    {
        public SimplePipettorInstrument() :
            base(new IndependentChannelPipettor(), GetDeck())
        {

        }

        private static IDeck GetDeck()
        {
            var deckPositions = new List<DeckPosition>();
            int numPositions = 8;
            for (int i = 0; i < numPositions; ++i)
            {
                //just temporary position assignement
                deckPositions.Add(new DeckPosition(i + 1,
                    new Coordinates(1.0, 1.0, 1.0),
                    new Coordinates(i, i, i)));
            }

            return new Deck(deckPositions);
        }
    }
}