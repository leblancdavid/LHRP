using System;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument.LiquidManagement;
using LHRP.Api.Instrument.TipManagement;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Resources;

namespace LHRP.Api.Instrument
{
    public interface IInstrument
    {
        IDeck Deck { get; }
        ITipManager TipManager { get; }
        ILiquidManager LiquidManager { get; }
        IPipettor Pipettor { get; }
        IDevice GetDevice(Guid id);
        Coordinates WastePosition { get; }

        void InitializeResources(ResourcesUsage resources);
    }
}