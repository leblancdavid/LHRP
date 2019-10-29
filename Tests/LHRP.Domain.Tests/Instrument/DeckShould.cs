using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Instrument;
using LHRP.Api.Labware;
using LHRP.Api.Labware.Plates;
using LHRP.Api.Labware.Tips;
using Xunit;

namespace LHRP.Domain.Tests.Instrument
{
  public class DeckShould : IDisposable
  {
      Deck deck;
    public DeckShould()
    {
        deck = new Deck(new List<DeckPosition>()
            {
                new DeckPosition(1, new Coordinates(0,0,0), new Coordinates(0,0,0)),
                new DeckPosition(2, new Coordinates(0,0,0), new Coordinates(0,0,0)),
                new DeckPosition(3, new Coordinates(0,0,0), new Coordinates(0,0,0)),
                new DeckPosition(4, new Coordinates(0,0,0), new Coordinates(0,0,0)),
            });
    }
    [Fact]
    public void Successfully_RetrieveAPositionById()
    {
      var position = deck.GetDeckPosition(1);
      position.IsSuccess.Should().BeTrue();
      position.Value.PositionId.Should().Be(1);
    }

    [Fact]
    public void Fail_WhenRetrievingAnInvalidPosition()
    {
      var position = deck.GetDeckPosition(99);
      position.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Successfully_AssignLabware()
    {
        var result = deck.AssignLabware(1, new Plate(new PlateDefinition("Plate1", new WellDefinition(), 8, 12, new Coordinates(0,0,0), 9.0)));
        result.IsSuccess.Should().BeTrue();
        var position = deck.GetDeckPosition(1);
        position.IsSuccess.Should().BeTrue();
        position.Value.IsOccupied.Should().BeTrue();
        (position.Value.AssignedLabware is Plate).Should().BeTrue();
    }

    [Fact]
    public void Fail_WhenAssigningLabware_ToAnInvalidPosition()
    {
        var result = deck.AssignLabware(99, new Plate(new PlateDefinition("Plate1", new WellDefinition(), 8, 12, new Coordinates(0,0,0), 9.0)));
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Successfully_RetrieveAllTipRacks()
    {
        deck.AssignLabware(1, new TipRack(new TipRackDefinition(300, "whatever1", 33, true, 8,12, new Coordinates(0,0,0), 9.0)));
        deck.AssignLabware(2, new TipRack(new TipRackDefinition(300, "whatever2", 33, true, 8,12, new Coordinates(0,0,0), 9.0)));
        var tipRacks = deck.GetTipRacks().ToList();
        tipRacks.Count.Should().Be(2);
        tipRacks[0].Definition.DisplayName.Should().Be("whatever1");
        tipRacks[1].Definition.DisplayName.Should().Be("whatever2");
    }

    [Fact]
    public void Successfully_RetrieveCoordinates_GivenAPositionAndAddess()
    {
      deck.AssignLabware(1, new TipRack(new TipRackDefinition(300, "whatever1", 33, true, 8,12, new Coordinates(0,0,0), 9.0)));
      var coordinates = deck.GetCoordinates(new LabwareAddress(1, 1, 1));
      coordinates.IsSuccess.Should().BeTrue();
      coordinates.Value.X.Should().BeApproximately(0.0, 0.0001);
      coordinates.Value.Y.Should().BeApproximately(0.0, 0.0001);
      coordinates.Value.Z.Should().BeApproximately(0.0, 0.0001);
    }

    [Fact]
    public void Fail_WhenRetrievingCoordinates_OfInvalidPositionsOrAddress()
    {
      deck.AssignLabware(1, new TipRack(new TipRackDefinition(300, "whatever1", 33, true, 8,12, new Coordinates(0,0,0), 9.0)));
      var coordinates = deck.GetCoordinates(new LabwareAddress(1, 1, 999));
      coordinates.IsSuccess.Should().BeFalse();
      coordinates = deck.GetCoordinates(new LabwareAddress(999, 999, 1));
      coordinates.IsSuccess.Should().BeFalse();
    }

    public void Dispose()
    {
      //do nothing
    }
  }
}