﻿using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Liquids;
using System;
using System.Linq;

namespace LHRP.Api.Runtime.Resources
{
    public class DefaultResoureAutoInitializer : IResourceInitializer
    {
        public Result Initialize(IInstrument instrument, ResourcesUsage resources)
        {
            var result = Result.Success();
            var liquidContainers = instrument.Deck.GetLiquidContainers();
            foreach (var liquid in resources.ConsumableLiquidUsages)
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

            foreach (var unknownLiquid in resources.LiquidContainerUsages.Where(x => x.RequiresLiquidAtStart))
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

            var tipRacks = instrument.Deck.GetTipRacks();
            foreach (var tip in resources.TipUsages)
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
    }
}
