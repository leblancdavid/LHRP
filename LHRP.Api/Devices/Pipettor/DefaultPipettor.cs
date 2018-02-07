using System;
using LHRP.Api.Common;
using LHRP.Api.Runtime;

namespace LHRP.Api.Devices.Pipettor
{
    public class DefaultPipettor : IPipettor
    {
         public Guid DeviceId { get; private set; }
        public int NumberChannels { get; private set; }
        public IDeviceStatus DeviceStatus { get; }
        public bool IsInitialized => throw new NotImplementedException();

        private ICommandProcessor _commandProcessor;

        public DefaultPipettor(ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        public Result<Process> Aspirate(AspirateCommand parameters)
        {
            return Result<Process>.Ok(new Process(new TimeSpan(), new TimeSpan()));
        }

        public Result<Process> Dispense(DispenseCommand parameters)
        {
            return Result<Process>.Ok(new Process(new TimeSpan(), new TimeSpan()));
        }

        public Result<Process> PickupTips(TipPickupCommand parameters)
        {
            return Result<Process>.Ok(new Process(new TimeSpan(), new TimeSpan()));
        }

        public Result<Process> DropTips(TipDropCommand parameters)
        {
            return Result<Process>.Ok(new Process(new TimeSpan(), new TimeSpan()));
        }

        public Result<Process> Initialize()
        {
            throw new NotImplementedException();
        }

        public Result<Process> Deinitialize()
        {
            throw new NotImplementedException();
        }
    }
}