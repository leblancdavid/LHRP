using System;
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Scheduling;

namespace LHRP.Api.Instrument
{
    public interface IInstrument : IStateSnapshotGetter<IInstrument>
    {
        IDeck Deck { get; }
        ITipManager TipManager { get; }
        ILiquidManager LiquidManager { get; }
        IPipettor Pipettor { get; }
        IDevice GetDevice(Guid id);
        Coordinates WastePosition { get; }

        Result<Schedule> InitializeResources(Schedule schedule);
    }
}