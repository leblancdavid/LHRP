using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Liquids;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Scheduling;
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

        public Result<Schedule> InitializeResources(Schedule schedule)
        {
            var result = Result.Success(schedule);
            var liquidContainers = _deck.GetLiquidContainers();
            foreach (var liquid in schedule.ResourcesUsage.ConsumableLiquidUsages)
            {
                var containers = liquidContainers.Where(x => x.ContainsLiquid(liquid.Key)).ToList();
                if(!containers.Any())
                {
                    Result.Combine(result, Result.Failure($"Unable to initialize liquid resource {liquid.Key.GetId()}, no container has been assigned for this liquid"));
                    continue;
                }

                double volume = liquid.Value;
                int containerIndex = 0;
                while (containerIndex < containers.Count())
                {
                    var c = containers[containerIndex];
                    if (c.MaxVolume < volume)
                    {
                        volume -= c.MaxVolume;
                        c.AddLiquid(liquid.Key, c.MaxVolume);
                    } else
                    {
                        c.AddLiquid(liquid.Key, volume);
                        break;
                    }
                    containerIndex++;
                }
            }

            foreach(var unknownLiquid in schedule.ResourcesUsage.LiquidContainerUsages.Where(x => x.RequiresLiquidAtStart))
            {
                var container = liquidContainers.FirstOrDefault(x => x.Address == unknownLiquid.Address);
                if(container == null)
                {
                    Result.Combine(result, Result.Failure($"Unable to initialize source liquid container at {unknownLiquid.Address}"));
                }

                if(container.Volume < unknownLiquid.RequiredLiquidVolumeAtStart)
                {
                    container.AddLiquid(new Liquid(), unknownLiquid.RequiredLiquidVolumeAtStart - container.Volume);
                }
            }

            var tipRacks = _deck.GetTipRacks();
            foreach(var tip in schedule.ResourcesUsage.TipUsages)
            {
                var tr = tipRacks.FirstOrDefault(x => x.Definition.Id == tip.TipTypeId);
                if (tr != null)
                {
                    if(tr.RemainingTips < tip.ExpectedTotalTipUsage)
                    {
                        tr.Refill();
                    }
                }
                else
                {
                    Result.Combine(result, Result.Failure($"Unable to initialize tip resource {tip.TipTypeId}, tip rack not found"));
                }
            }
            return result;
        }

        public IPipettor Pipettor
        {
            get { return _pipettor; }
        }
    }
}