using System;
using System.Collections.Generic;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Instrument.LiquidManagement;
using LHRP.Api.Instrument.TipManagement;
using LHRP.Api.Runtime;
using LHRP.Instrument.NimbusLite.Devices.Pipettor;

namespace LHRP.Instrument.NimbusLite.Instrument
{
  public class NimbusLiteSimulatedInstrument : IInstrument, ISimulation
  {
    IndependentChannelSimulatedPipettor _pipettor;

    private uint _simulationSpeedFactor;
    public uint SimulationSpeedFactor
    {
      get
      {
        return _simulationSpeedFactor;
      }
      set
      {
        _simulationSpeedFactor = value;
        _pipettor.SimulationSpeedFactor = value;
      }
    }

    private IDeck _deck;
    public IDeck Deck
    {
      get
      {
        return _deck;
      }
    }
    private TipManager _tipManager;
    public ITipManager TipManager => _tipManager;
    
    private LiquidManager _liquidManager;
    public ILiquidManager LiquidManager => _liquidManager;
    private Coordinates _wastePosition = new Coordinates(0.0, 0.0, 0.0);
    public Coordinates WastePosition
    {
      get
      {
        return _wastePosition;
      }
    }

    public double FailureRate { get; set; }
    public NimbusLiteSimulatedInstrument()
    {
      _pipettor = new IndependentChannelSimulatedPipettor();

      var deckPositions = new List<DeckPosition>();
      int numPositions = 8;
      for (int i = 0; i < numPositions; ++i)
      {
        //just temporary position assignement
        deckPositions.Add(new DeckPosition(i + 1,
            new Coordinates(1.0, 1.0, 1.0),
            new Coordinates(i, i, i)));
      }

      _deck = new Deck(deckPositions);
      _tipManager = new TipManager(_deck);
      _liquidManager = new LiquidManager(_deck);
    }

    public IDevice GetDevice(Guid id)
    {
      throw new System.NotImplementedException();
    }

    public IPipettor Pipettor
    {
      get { return _pipettor; }
    }
  }
}