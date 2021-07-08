using FluentAssertions;
using LHRP.Api.Liquids;
using System.Linq;
using Xunit;

namespace LHRP.Domain.Tests.Liquids
{
    public class HeterogeneousLiquidShould
    {
        [Fact]
        public void MatchToOtherIdenticalLiquids()
        {
            var liquid1 = new HeterogeneousLiquid("TestLiquid");
            var liquid2 = new HeterogeneousLiquid("TestLiquid");
            liquid1.Match(liquid2).Should().BeTrue();
        }

        [Fact]
        public void MatchToOtherIdenticalBaseLiquids()
        {
            var liquid1 = new HeterogeneousLiquid("TestLiquid");
            var liquid2 = new Liquid("TestLiquid");
            liquid1.Match(liquid2).Should().BeTrue();
        }

        [Fact]
        public void NotMatchToOtherLiquids()
        {
            var liquid1 = new HeterogeneousLiquid("TestLiquid");
            var liquid2 = new HeterogeneousLiquid("AnotherLiquid");
            liquid1.Match(liquid2).Should().BeFalse();
        }

        [Fact]
        public void MixWithAnotherLiquid()
        {
            var liquid = new HeterogeneousLiquid("Liquid1");
            liquid.Mix(new HeterogeneousLiquid("Liquid2"), 0.5);

            liquid.LiquidParts.Count().Should().Be(2);
            liquid.LiquidParts.ToArray()[0].Concentration.Should().BeApproximately(0.5, 0.00001);
            liquid.LiquidParts.ToArray()[1].Concentration.Should().BeApproximately(0.5, 0.00001);
        }

        [Fact]
        public void ResultInTheSameLiquidWhenMixedWithItself()
        {
            var liquid = new HeterogeneousLiquid("Liquid1");
            liquid.Mix(new HeterogeneousLiquid("Liquid1"), 0.5);

            liquid.LiquidParts.Count().Should().Be(1);
            liquid.LiquidParts.ToArray()[0].Concentration.Should().BeApproximately(1.0, 0.00001);
        }

        [Fact]
        public void MixWithMultipleHeterogeneousLiquid()
        {
            var mixedLiquid = new HeterogeneousLiquid("Liquid1");
            mixedLiquid.Mix(new HeterogeneousLiquid("Liquid2"), 0.5);
            mixedLiquid.Mix(new HeterogeneousLiquid("Liquid3"), 0.5);

            mixedLiquid.LiquidParts.Count().Should().Be(3);
            mixedLiquid.LiquidParts.ToArray()[0].Concentration.Should().BeApproximately(0.25, 0.00001);
            mixedLiquid.LiquidParts.ToArray()[1].Concentration.Should().BeApproximately(0.25, 0.00001);
            mixedLiquid.LiquidParts.ToArray()[2].Concentration.Should().BeApproximately(0.5, 0.00001);
        }

        [Fact]
        public void MixWithMultipleHeterogeneousLiquid2()
        {
            var mixedLiquid1 = new HeterogeneousLiquid("Liquid1");
            mixedLiquid1.Mix(new HeterogeneousLiquid("Liquid2"), 0.5);
            var mixedLiquid2 = new HeterogeneousLiquid("Liquid3");
            mixedLiquid2.Mix(new HeterogeneousLiquid("Liquid4"), 0.5);

            mixedLiquid1.Mix(mixedLiquid2, 0.5);
            mixedLiquid1.LiquidParts.Count().Should().Be(4);
            mixedLiquid1.LiquidParts.ToArray()[0].Concentration.Should().BeApproximately(0.25, 0.00001);
            mixedLiquid1.LiquidParts.ToArray()[1].Concentration.Should().BeApproximately(0.25, 0.00001);
            mixedLiquid1.LiquidParts.ToArray()[2].Concentration.Should().BeApproximately(0.25, 0.00001);
            mixedLiquid1.LiquidParts.ToArray()[3].Concentration.Should().BeApproximately(0.25, 0.00001);
        }

    }
}
