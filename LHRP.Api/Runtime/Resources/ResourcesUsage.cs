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

        public ResourcesUsage()
        {
            _liquidContainerUsages = new Dictionary<LabwareAddress, LiquidContainerUsage>();
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
    }
}