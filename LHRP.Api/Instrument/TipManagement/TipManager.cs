using System;
using System.Collections.Generic;
using LHRP.Api.Common;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Instrument.TipManagement 
{
    public class TipManager : ITipManager
    {
        private Dictionary<int, TipRack> _tipRacks = new Dictionary<int, TipRack>();
        public TipManager()
        {

        }

        public Result AssignTipRack(int positionId, TipRack tipRack)
        {
            
            throw new System.NotImplementedException();
        }

        public Result<TipChannelPattern> RequestTips(ChannelPattern pattern, double tipSize)
        {
            TipRack availableTipRack = null;
            foreach(var tr in _tipRacks.Values)
            {
                //Probably will need more rules here
                if(tr.Definition.TipVolume == tipSize && 
                    tr.RemainingTips <= pattern.GetNumberActiveChannels())
                {
                    availableTipRack = tr;
                    break;
                }
            }

            if(availableTipRack == null)
            {
                return Result<TipChannelPattern>.Fail($"Insufficient number of tips of size {tipSize} for channel pattern {pattern.ToString()}");
            }

            var tipChannelPattern = new TipChannelPattern(pattern.NumChannels);
            for(int i = 0; i < pattern.NumChannels; ++i)
            {
                if(pattern[i])
                {
                    var nextTip = availableTipRack.GetNextAvailableTip();
                    //this should never happen
                    if(nextTip.IsFailure)
                    {
                        return Result<TipChannelPattern>.Fail($"Unable to retrieve next available tip on tip-rack position {availableTipRack.PositionId}");
                    }
                    tipChannelPattern.SetTip(i, nextTip.Value);
                }
            }

            return Result<TipChannelPattern>.Ok(tipChannelPattern);
        }
    }
}