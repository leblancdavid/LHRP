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
    }
}