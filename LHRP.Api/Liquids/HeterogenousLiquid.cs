﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Liquids
{
    public class HeterogeneousLiquid : Liquid
    {
        private List<LiquidPart> _liquidParts;

        public IEnumerable<LiquidPart> LiquidParts => _liquidParts;

        public HeterogeneousLiquid()
        {
            _liquidParts = new List<LiquidPart>();
        }

        public HeterogeneousLiquid(string name)
        {
            _liquidParts = new List<LiquidPart>();
            _liquidParts.Add(new LiquidPart(name));
        }

        public HeterogeneousLiquid(Liquid initialLiquid)
        {
            _liquidParts = new List<LiquidPart>();
            Mix(initialLiquid, 1.0);
        }

        public bool IsPure()
        {
            return _liquidParts.Count <= 1;
        }

        public override bool ContainsLiquid(Liquid liquid)
        {
            if (GetId() == liquid.GetId() || _liquidParts.Any(x => x.GetId() == liquid.GetId()))
                return true;

            return false;
        }

        public override HeterogeneousLiquid Mix(Liquid liquid, double ratio)
        {
            if(Match(liquid))
            {
                return this;
            }

            if (ratio > 1.0)
            {
                ratio = 1.0;
            }
            var r_0 = 1.0 - ratio;
            foreach(var part in _liquidParts)
            {
                part.Concentration *= r_0;
            }

            _liquidParts.RemoveAll(x => Math.Round(x.Concentration, x.ConcentrationPrecision) == 0.0);

            if(liquid is HeterogeneousLiquid)
            {
                var hLiquid = liquid as HeterogeneousLiquid;
                foreach(var part in hLiquid!.LiquidParts)
                {
                    var duplicateLiquid = _liquidParts.FirstOrDefault(x => x.Match(part));
                    if(duplicateLiquid != null)
                    {
                        duplicateLiquid.Concentration += part.Concentration * ratio;
                    }
                    else
                    {
                        _liquidParts.Add(new LiquidPart(part.GetId(), part.Concentration * ratio));
                    }
                }
            }
            else
            {
                var duplicateLiquid = _liquidParts.FirstOrDefault(x => x.Match(liquid));
                if (duplicateLiquid != null)
                {
                    duplicateLiquid.Concentration += ratio;
                }
                else
                {
                    _liquidParts.Add(new LiquidPart(liquid.GetId(), ratio));
                }
            }

            return this;
        }

        public override string GetId()
        {
            return String.Join("-", _liquidParts.Select(x => x.GetIdWithConcentration()));
        }
    }
}
