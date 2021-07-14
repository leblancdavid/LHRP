using System;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;

namespace LHRP.Api.Instrument
{
    public interface IInstrument : IStateSnapshotGetter<IInstrument>, ISimulatable<IInstrument>
    {
        IDeck Deck { get; }
        ITipManager TipManager { get; }
        ILiquidManager LiquidManager { get; }
        IPipettor Pipettor { get; }
        IDevice GetDevice(Guid id);
        Coordinates WastePosition { get; }
    }
}