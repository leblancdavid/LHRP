using System;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime;

namespace LHRP.Api.Instrument
{
    public interface IInstrument
    {
        IDeck Deck { get; }
        IPipettor Pipettor { get; }
        IDevice GetDevice(Guid id);

        Coordinates WastePosition { get; }
    }
}