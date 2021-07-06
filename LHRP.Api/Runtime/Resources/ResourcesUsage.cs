using CSharpFunctionalExtensions;
using LHRP.Api.Labware;
using LHRP.Api.Protocol.Transfers;
using System.Collections.Generic;

namespace LHRP.Api.Runtime.Resources
{
    public class ResourcesUsage
    {
        private Dictionary<LabwareAddress, LiquidContainerUsage> _liquidContainerUsages;
        public IEnumerable<LiquidContainerUsage> LiquidContainerUsages
        {
            get
            {
                return _liquidContainerUsages.Values;
            }
        }

        private Dictionary<int, TipUsage> _tipUsages;
        public IEnumerable<TipUsage> TipUsages
        {
            get
            {
                return _tipUsages.Values;
            }
        }

        private Dictionary<Liquids.Liquid, double> _consumableLiquidUsages;
        public IReadOnlyDictionary<Liquids.Liquid, double> ConsumableLiquidUsages => _consumableLiquidUsages;

        public ResourcesUsage()
        {
            _liquidContainerUsages = new Dictionary<LabwareAddress, LiquidContainerUsage>();
            _consumableLiquidUsages = new Dictionary<Liquids.Liquid, double>();
            _tipUsages = new Dictionary<int, TipUsage>();
        }

        public Result AddTransfer(TransferTarget target)
        {
            if(_liquidContainerUsages.ContainsKey(target.Address))
            {
                return _liquidContainerUsages[target.Address].AddTransfer(target);
            }
            else
            {
                var newContainer = new LiquidContainerUsage(target.Address);
                var result = newContainer.AddTransfer(target);
                if (result.IsFailure)
                    return result;

                _liquidContainerUsages[target.Address] = newContainer;
                return result;
            }
        }

        public Result AddConsumableLiquidUsage(Liquids.Liquid liquid, double volume)
        {
            if(!_consumableLiquidUsages.ContainsKey(liquid))
            {
                _consumableLiquidUsages.Add(liquid, volume);
            }
            else
            {
                _consumableLiquidUsages[liquid] += volume;
            }
            return Result.Ok();
        }

        public void AddTipUsage(int tipTypeId, int count)
        {
            if (_tipUsages.ContainsKey(tipTypeId))
            {
                _tipUsages[tipTypeId].ExpectedTotalTipUsage += count;
            }
            else
            {
                var newTipUsage = new TipUsage(tipTypeId);
                newTipUsage.ExpectedTotalTipUsage += count;
                _tipUsages[tipTypeId] = newTipUsage;
            }
        }

        public void Combine(params ResourcesUsage[] resourcesUsages)
        {
            for(int i = 0; i < resourcesUsages.Length; ++i)
            {
                foreach(var liquidContainerUsage in resourcesUsages[i].LiquidContainerUsages)
                {
                    foreach(var transfer in liquidContainerUsage.TransferHistory)
                    {
                        AddTransfer(transfer);
                    }
                }

                foreach(var tipUsage in resourcesUsages[i].TipUsages)
                {
                    AddTipUsage(tipUsage.TipTypeId, tipUsage.ExpectedTotalTipUsage);
                }

                foreach(var consumable in resourcesUsages[i].ConsumableLiquidUsages)
                {
                    AddConsumableLiquidUsage(consumable.Key, consumable.Value);
                }
            }
        }
    }
}