using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware;
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

        public Result ConsumeTip(Tip tip)
        {
            var position = _deck.GetDeckPosition(tip.Address.PositionId);
            if(position.IsFailure)
            {
                return position;
            }

            if(!position.Value.IsOccupied || !(position.Value.AssignedLabware is TipRack))
            {
                return Result.Fail($"Position Id {tip.Address.PositionId} does not contain a tip rack");
            }

            var tipRack = position.Value.AssignedLabware as TipRack;
            return tipRack.Consume(tip);
        }

        public Result<TipChannelPattern> RequestTips(ChannelPattern pattern, int tipTypeId)
        {
            var tipRacks = _deck.GetTipRacks();
            TipRack availableTipRack = null;
            foreach(var tr in tipRacks)
            {
                //Probably will need more rules here
                if(tr.Definition.Id == tipTypeId && 
                    tr.RemainingTips >= pattern.GetNumberActiveChannels())
                {
                    availableTipRack = tr;
                    break;
                }
            }

            if(availableTipRack == null)
            {
                return Result.Fail<TipChannelPattern>($"Insufficient number of tips of type {tipTypeId} for channel pattern {pattern.GetChannelString()}");
            }
            var availableTips = availableTipRack.GetNextAvailableTips(pattern.GetNumberActiveChannels());
            if(availableTips.IsFailure)
            {
                return Result.Fail<TipChannelPattern>($"Insufficient number of tips of type {tipTypeId} for channel pattern {pattern.GetChannelString()}");
            }

            var tipChannelPattern = new TipChannelPattern(pattern.NumChannels);
            int t = 0;
            for(int i = 0; i < pattern.NumChannels; ++i)
            {
                if(pattern[i])
                {
                    tipChannelPattern.SetTip(i, availableTips.Value[t]);
                    ++t;
                }
            }

            return Result.Ok<TipChannelPattern>(tipChannelPattern);
        }

        public Result ReloadTips(int tipTypeId)
        {
            var tipRacks = _deck.GetTipRacks().Where(tr => tr.Definition.Id == tipTypeId);
            if(tipRacks.Count() == 0)
            {
                return Result.Fail($"No tip rack with definition ID {tipTypeId} found");
            }

            foreach(var tipRack in tipRacks)
            {
                tipRack.Refill();
            }

            return Result.Ok();
        }
    }
}