using System;
using System.Collections.Generic;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Instrument.NimbusLite.Devices.Pipettor;

namespace LHRP.Instrument.NimbusLite.Instrument
{
    public class NimbusLiteInstrument : IInstrument
    {
        private IPipettor _pipettor;
        
        public NimbusLiteInstrument()
        {
            _pipettor = new IndependentChannelPipettor();

            var deckPositions = new List<DeckPosition>();
            int numPositions = 8;
            for(int i = 0; i < numPositions; ++i)
            {
                //just temporary position assignement
                deckPositions.Add(new DeckPosition(i+1,
                    new Coordinates(1.0, 1.0, 1.0),
                    new Coordinates(i, i, i)));
            }
            
            _deck = new Deck(deckPositions);
        }

        private IDeck _deck;
        public IDeck Deck 
        { 
            get
            {
                return _deck;
            }
        }

        public IDevice GetDevice(Guid id)
        {
        throw new System.NotImplementedException();
        }

        public IPipettor GetPipettor()
        {
            return _pipettor;
        }
    }
}