using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware;

namespace LHRP.Api.Instrument
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
            if(position == null)
            {
                return Result.Failure($"No deck position found at address {tip.Address.ToAlphaAddress()}");
            }

            var tipRack = position.AssignedLabware as TipRack;
            if (tipRack == null)
            {
                return Result.Failure($"Position Id {tip.Address.PositionId} does not contain a tip rack");
            }

            return tipRack.Consume(tip);
        }

        public Result<TipChannelPattern> RequestTips(ChannelPattern pattern, int tipTypeId)
        {
            var tipRacks = _deck.GetTipRacks();
            TipRack? availableTipRack = null;
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
                return Result.Failure<TipChannelPattern>($"Insufficient number of tips of type {tipTypeId} for channel pattern {pattern.GetChannelString()}");
            }
            var availableTips = availableTipRack.GetNextAvailableTips(pattern.GetNumberActiveChannels());
            if(availableTips.IsFailure)
            {
                return Result.Failure<TipChannelPattern>($"Insufficient number of tips of type {tipTypeId} for channel pattern {pattern.GetChannelString()}");
            }

            var tipChannelPattern = new TipChannelPattern(pattern.NumChannels);
            int t = 0;
            for(int i = 0; i < pattern.NumChannels; ++i)
            {
                if(pattern.IsInUse(i))
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
                return Result.Failure($"No tip rack with definition ID {tipTypeId} found");
            }

            foreach(var tipRack in tipRacks)
            {
                tipRack.Refill();
            }

            return Result.Ok();
        }

        public int GetTotalTipCount(int tipTypeId)
        {
            int totalCount = 0;
            var tipRacks = _deck.GetTipRacks();
            foreach (var tr in tipRacks)
            {
                if(tr.Definition.Id == tipTypeId)
                {
                    totalCount += tr.TotalTipCount;
                }
            }

            return totalCount;
        }

        public double GetTipCapacity(int tipTypeId)
        {
            var tipRacks = _deck.GetTipRacks();
            foreach (var tr in tipRacks)
            {
                if (tr.Definition.Id == tipTypeId)
                {
                    return tr.Definition.TipVolume;
                }
            }

            return -1;
        }

        public ITipManager GetSnapshot()
        {
            throw new System.NotImplementedException();
        }
    }
}