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
    public class SimplePipettorInstrument : IInstrument
    {
        
        public SimplePipettorInstrument()
        {
            Pipettor = new IndependentChannelPipettor();

            var deckPositions = new List<DeckPosition>();
            int numPositions = 8;
            for(int i = 0; i < numPositions; ++i)
            {
                //just temporary position assignement
                deckPositions.Add(new DeckPosition(i+1,
                    new Coordinates(1.0, 1.0, 1.0),
                    new Coordinates(i, i, i)));
            }
            
            InitializeDeck(new Deck(deckPositions));
        }

        public IPipettor Pipettor { get; private set; }
        public IDeck Deck { get; private set; }
        public ITipManager TipManager { get; private set; }
        public ILiquidManager LiquidManager { get; private set; }

        private Coordinates _wastePosition = new Coordinates(0.0, 0.0, 0.0);
        public Coordinates WastePosition
        {
            get
            {
                return _wastePosition;
            }
        }

        public IDevice GetDevice(Guid id)
        {
            throw new System.NotImplementedException();
        }

        private void InitializeDeck(IDeck deck)
        {
            Deck = deck;
            TipManager = new TipManager(Deck);
            LiquidManager = new LiquidManager(Deck);
        }

        public Result<Schedule> InitializeResources(Schedule schedule)
        {
            throw new NotImplementedException();
        }

        public IInstrument GetSnapshot()
        {
            var deck = this.Deck.GetSnapshot();
            return new SimplePipettorInstrument()
            {
                Deck = deck,
                TipManager = new TipManager(deck),
                LiquidManager = new LiquidManager(deck),
                Pipettor = this.Pipettor
            };
        }

  }
}