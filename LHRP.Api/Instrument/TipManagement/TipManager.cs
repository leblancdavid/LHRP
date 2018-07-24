using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Instrument.TipManagement 
{
    public class TipManager : ITipManager
    {
        private IDeck _deck;
        public TipManager(IDeck deck)
        {
            _deck = deck;
        }

        public Result<TipChannelPattern> RequestTips(ChannelPattern pattern, double tipSize)
        {
            var tipRacks = _deck.GetTipRacks();
            TipRack availableTipRack = null;
            foreach(var tr in tipRacks)
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
                return Result.Fail<TipChannelPattern>($"Insufficient number of tips of size {tipSize} for channel pattern {pattern.ToString()}");
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
                        return Result.Fail<TipChannelPattern>($"Unable to retrieve next available tip on tip-rack position {availableTipRack.PositionId}");
                    }
                    tipChannelPattern.SetTip(i, nextTip.Value);
                }
            }

            return Result.Ok<TipChannelPattern>(tipChannelPattern);
        }
    }
}