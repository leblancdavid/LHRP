using System;
using System.Collections.Generic;
using System.Linq;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Instrument.LiquidManagement;
using LHRP.Api.Instrument.TipManagement;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Resources;
using LHRP.Instrument.SimplePipettor.Devices.Pipettor;

namespace LHRP.Instrument.SimplePipettor.Instrument
{
    public class SimplePipettorSimulatedInstrument : IInstrument, ISimulation
    {
        IndependentChannelSimulatedPipettor _pipettor;

        private uint _simulationSpeedFactor;
        public uint SimulationSpeedFactor
        {
            get
            {
                return _simulationSpeedFactor;
            }
            set
            {
                _simulationSpeedFactor = value;
                _pipettor.SimulationSpeedFactor = value;
            }
        }

        private IDeck _deck;
        public IDeck Deck
        {
            get
            {
                return _deck;
            }
        }
        private TipManager _tipManager;
        public ITipManager TipManager => _tipManager;

        private LiquidManager _liquidManager;
        public ILiquidManager LiquidManager => _liquidManager;
        private Coordinates _wastePosition = new Coordinates(0.0, 0.0, 0.0);
        public Coordinates WastePosition
        {
            get
            {
                return _wastePosition;
            }
        }

        public double FailureRate { get; set; }
        public SimplePipettorSimulatedInstrument()
        {
            _pipettor = new IndependentChannelSimulatedPipettor();

            var deckPositions = new List<DeckPosition>();
            int numPositions = 8;
            for (int i = 0; i < numPositions; ++i)
            {
                //just temporary position assignement
                deckPositions.Add(new DeckPosition(i + 1,
                    new Coordinates(1.0, 1.0, 1.0),
                    new Coordinates(i, i, i)));
            }

            InitializeDeck(new Deck(deckPositions));
        }

        public IDevice GetDevice(Guid id)
        {
            throw new System.NotImplementedException();
        }

        private void InitializeDeck(IDeck deck)
        {
            _deck = deck;
            _tipManager = new TipManager(_deck);
            _liquidManager = new LiquidManager(new LiquidManagerConfiguration(true), _deck);
        }

        public void InitializeResources(ResourcesUsage resources)
        {
            var liquidContainers = _deck.GetLiquidContainers();
            foreach (var liquid in resources.ConsumableLiquidUsages)
            {
                var container = liquidContainers.FirstOrDefault(x => x.IsPure && x.ContainsLiquid(liquid.Key));
                if(container != null)
                {
                    container.AddLiquid(liquid.Key, liquid.Value);
                }
            }
        }

        public IPipettor Pipettor
        {
            get { return _pipettor; }
        }
    }
}