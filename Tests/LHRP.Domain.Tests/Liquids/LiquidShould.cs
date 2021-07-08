using FluentAssertions;
using LHRP.Api.Liquids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LHRP.Domain.Tests.Liquids
{
    public class LiquidShould
    {
        [Fact]
        public void MatchToOtherIdenticalLiquids()
        {
            var liquid1 = new Liquid("TestLiquid");
            var liquid2 = new Liquid("TestLiquid");
            liquid1.Match(liquid2).Should().BeTrue();
        }

        [Fact]
        public void MatchToOtherIdenticalHeterogeneousLiquids()
        {
            var liquid1 = new Liquid("TestLiquid");
            var liquid2 = new HeterogeneousLiquid();
            liquid2.Mix(new Liquid("TestLiquid"), 1.0);
            liquid1.Match(liquid2).Should().BeTrue();
        }

        [Fact]
        public void NotMatchToOtherLiquids()
        {
            var liquid1 = new Liquid("TestLiquid");
            var liquid2 = new Liquid();
            liquid1.Match(liquid2).Should().BeFalse();
        }

        [Fact]
        public void MixWithAnotherLiquid()
        {
            var liquid1 = new Liquid("Liquid1");
            var mixedLiquid = liquid1.Mix(new Liquid("Liquid2"), 0.5);

            mixedLiquid.LiquidParts.Count().Should().Be(2);
            mixedLiquid.LiquidParts.ToArray()[0].Concentration.Should().BeApproximately(0.5, 0.00001);
            mixedLiquid.LiquidParts.ToArray()[1].Concentration.Should().BeApproximately(0.5, 0.00001);
        }

        [Fact]
        public void ResultInTheSameLiquidWhenMixedWithItself()
        {
            var liquid1 = new Liquid("Liquid1");
            var mixedLiquid = liquid1.Mix(new Liquid("Liquid1"), 0.5);

            mixedLiquid.LiquidParts.Count().Should().Be(1);
            mixedLiquid.LiquidParts.ToArray()[0].Concentration.Should().BeApproximately(1.0, 0.00001);
            liquid1.Match(mixedLiquid).Should().BeTrue();
        }

        [Fact]
        public void MixWithAHeterogeneousLiquid()
        {
            var liquid1 = new Liquid("Liquid1");

            var mixedLiquid = new HeterogeneousLiquid();
            mixedLiquid.Mix(new Liquid("Liquid2"), 1.0);
            mixedLiquid.Mix(new Liquid("Liquid3"), 0.5);

            mixedLiquid = liquid1.Mix(mixedLiquid, 0.5);

            mixedLiquid.LiquidParts.Count().Should().Be(3);
            mixedLiquid.LiquidParts.ToArray()[0].Concentration.Should().BeApproximately(0.25, 0.00001);
            mixedLiquid.LiquidParts.ToArray()[1].Concentration.Should().BeApproximately(0.25, 0.00001);
            mixedLiquid.LiquidParts.ToArray()[2].Concentration.Should().BeApproximately(0.5, 0.00001);
        }
    }
}
