using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using System;

namespace LHRP.Api.Instrument
{
    public class BaseInstrument : IInstrument
    {
        public IDeck Deck { get; protected set; }

        public ITipManager TipManager { get; protected set; }

        public ILiquidManager LiquidManager { get; protected set; }

        public IPipettor Pipettor { get; protected set; }

        public Coordinates WastePosition { get; protected set; }

        public uint SimulationSpeedFactor { get; set; }
        public double FailureRate { get; set; }


        public BaseInstrument(IPipettor pipettor, IDeck deck)
        {
            Pipettor = pipettor;
            Deck = deck;
            TipManager = new TipManager(Deck);
            LiquidManager = new LiquidManager(Deck);
            WastePosition = new Coordinates();
        }

        public IDevice GetDevice(Guid id)
        {
            throw new NotImplementedException();
        }

        public IInstrument GetSnapshot()
        {
            var deck = this.Deck.GetSnapshot();
            return new BaseInstrument(Pipettor, deck);
        }

        public IInstrument GetSimulation()
        {
            return new BaseInstrument(Pipettor.GetSimulation(), Deck.GetSnapshot());
        }
    }
}
