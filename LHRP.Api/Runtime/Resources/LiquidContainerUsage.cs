using CSharpFunctionalExtensions;
using LHRP.Api.Instrument;
using LHRP.Api.Labware;
using LHRP.Api.Protocol.Transfers;
using System.Collections.Generic;
using System.Linq;

namespace LHRP.Api.Runtime.Resources
{
    public class LiquidContainerUsage
    {
        public LabwareAddress Address { get; private set; }

        private List<TransferTarget> _transferHistory = new List<TransferTarget>();
        public IEnumerable<TransferTarget> TransferHistory => _transferHistory;

        public bool RequiresLiquidAtStart
        {
            get
            {
                var firstTransfer = _transferHistory.FirstOrDefault();
                if(firstTransfer == null || firstTransfer.TransferType == TransferType.Destination)
                {
                    return false;
                }
                return true;
            }
        }

        public double RequiredLiquidVolumeAtStart
        {
            get
            {
                double volume = 0.0;
                for(int i = 0; i < _transferHistory.Count; ++i)
                {
                    var target = _transferHistory[i];
                    if (target.TransferType == TransferType.Destination)
                    {
                        break;
                    }
                    volume += target.Volume;
                }
                return volume;
            }
        }

        public bool RequiresReloading
        {
            get
            {
                double volume = 0.0;
                int i;
                //Ignore liquid that will be present in the beginning
                for (i = 0; i < _transferHistory.Count; ++i)
                {
                    var target = _transferHistory[i];
                    if (target.TransferType == TransferType.Destination)
                    {
                        break;
                    }
                }

                //if from that point, the total volume goes negative, it will require reloading
                for(; i < _transferHistory.Count; ++i)
                {
                    var target = _transferHistory[i];
                    if (target.TransferType == TransferType.Destination)
                    {
                        volume += target.Volume;
                    }
                    else
                    {
                        volume -= target.Volume;
                    }

                    if (volume < 0.0)
                        return true;
                }

                return false;
            }
        }

        public double ExpectedFinalLiquidVolume
        {
            get
            {
                double volume = 0.0;
                for (int i = 0; i < _transferHistory.Count; ++i)
                {
                    var target = _transferHistory[i];
                    if (target.TransferType == TransferType.Destination)
                        volume += target.Volume;
                    else
                        volume -= target.Volume;
                }
                return volume + RequiredLiquidVolumeAtStart;
            }
        }

        public LiquidContainerUsage(LabwareAddress address)
        {
            Address = address;
        }

        public Result AddTransfer(TransferTarget target)
        {
            if(target.Address != Address)
            {
                return Result.Fail($"Unable to add TransferTarget to WellUsage at address {Address.ToString()}: Invalid transfer target address {target.Address.ToString()}");
            }

            _transferHistory.Add(target);
            return Result.Ok();
        }

        public Result InitializeInstrumentState(IInstrument instrument)
        {
            var clearResult = instrument.LiquidManager.ClearLiquidAtPosition(Address);
            if(clearResult.IsFailure)
            {
                return clearResult;
            }

            for (int i = 0; i < _transferHistory.Count; ++i)
            {
                var target = _transferHistory[i];
                if (target.TransferType == TransferType.Destination)
                {
                    break;
                }

                var addResult = instrument.LiquidManager.AddLiquidToPosition(target.Address, target.Liquid, target.Volume);
                if (addResult.IsFailure)
                {
                    return addResult;
                }
            }

            return Result.Ok();
        }

        public void Combine(params LiquidContainerUsage[] usages)
        {
            for(int i = 0; i < usages.Length; ++i)
            {
                if (!Address.Equals(usages[i].Address))
                    continue;

                _transferHistory.AddRange(usages[i].TransferHistory);
            }
        }
    }
}
