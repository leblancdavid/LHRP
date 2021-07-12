using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Liquids;
using LHRP.Api.Runtime.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Instrument
{
    public class Instrument : IInstrument
    {
        public IDeck Deck { get; protected set; }

        public ITipManager TipManager { get; protected set; }

        public ILiquidManager LiquidManager { get; protected set; }

        public IPipettor Pipettor { get; protected set; }

        public Coordinates WastePosition { get; protected set; }

        public uint SimulationSpeedFactor { get; set; }
        public double FailureRate { get; set; }


        public Instrument(IPipettor pipettor, IDeck deck)
        {
            Pipettor = pipettor;
            Deck = deck;
            TipManager = new TipManager(Deck);
            LiquidManager = new LiquidManager(Deck);
            WastePosition = new Coordinates();
        }

        public IDevice GetDevice(Guid id)
        {
            throw new NotImplementedException();
        }

        

        public Result<Schedule> InitializeResources(Schedule schedule)
        {
            var result = Result.Success(schedule);
            var liquidContainers = Deck.GetLiquidContainers();
            foreach (var liquid in schedule.ResourcesUsage.ConsumableLiquidUsages)
            {
                var containers = liquidContainers.Where(x => x.ContainsLiquid(liquid.Key)).ToList();
                if (!containers.Any())
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
                    }
                    else
                    {
                        c.AddLiquid(liquid.Key, volume);
                        break;
                    }
                    containerIndex++;
                }
            }

            foreach (var unknownLiquid in schedule.ResourcesUsage.LiquidContainerUsages.Where(x => x.RequiresLiquidAtStart))
            {
                var container = liquidContainers.FirstOrDefault(x => x.Address == unknownLiquid.Address);
                if (container == null)
                {
                    Result.Combine(result, Result.Failure($"Unable to initialize source liquid container at {unknownLiquid.Address}"));
                }

                if (container!.Volume < unknownLiquid.RequiredLiquidVolumeAtStart)
                {
                    container.AddLiquid(new Liquid(), unknownLiquid.RequiredLiquidVolumeAtStart - container.Volume);
                }
            }

            var tipRacks = Deck.GetTipRacks();
            foreach (var tip in schedule.ResourcesUsage.TipUsages)
            {
                var tr = tipRacks.FirstOrDefault(x => x.Definition.Id == tip.TipTypeId);
                if (tr != null)
                {
                    if (tr.RemainingTips < tip.ExpectedTotalTipUsage)
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

        public IInstrument GetSnapshot()
        {
            var deck = this.Deck.GetSnapshot();
            return new Instrument(Pipettor, deck);
        }

        public IInstrument GetSimulation()
        {
            return new Instrument(Pipettor.GetSimulation(), Deck.GetSnapshot());
        }
    }
}
