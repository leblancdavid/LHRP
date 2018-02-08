using System;
using System.Collections.Generic;
using System.Threading;
using LHRP.Api.Common;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;

namespace LHRP.Instrument.NimbusLite.Devices.Pipettor
{
    public class IndependentChannelSimulatedPipettor: IPipettor, ISimulation
    {
        private const double _motorSpeed = 100.0; //mm/s
        public uint SimulationSpeedFactor { get; set; }
        public double FailureRate { get; set; }
        public int NumberChannels { get; private set; }
        private PipettorStatus _pipettorStatus;
        public IDeviceStatus DeviceStatus 
        { 
            get { return _pipettorStatus; } 
        }

        public Guid DeviceId { get; private set; }

        public IndependentChannelSimulatedPipettor()
        {
            NumberChannels = 1;
            _pipettorStatus = new PipettorStatus(NumberChannels);
            SimulationSpeedFactor = 1;
            FailureRate = 0;
        }

        public Result<Process> Aspirate(AspirateCommand parameters)
        {
            Console.WriteLine("Aspirating " + parameters.Volume + "uL from position: (" + 
                parameters.Position.X + ", " +
                parameters.Position.Y + ", " + 
                parameters.Position.Z + ")");

            var estimatedTime = GetTravelTimeToPosition(parameters.Position) + new TimeSpan(0, 0, 1);
            SimulateRuntimeWait(estimatedTime);

            _pipettorStatus.CurrentPosition = parameters.Position;

            return Result<Process>.Ok(new Process(estimatedTime, estimatedTime));
        }

        public Result<Process> Dispense(DispenseCommand parameters)
        {
            Console.WriteLine("Dispensing " + parameters.Volume + "uL to position: (" + 
                parameters.Position.X + ", " +
                parameters.Position.Y + ", " + 
                parameters.Position.Z + ")");

            var estimatedTime = GetTravelTimeToPosition(parameters.Position) + new TimeSpan(0, 0, 1);
            SimulateRuntimeWait(estimatedTime);

            _pipettorStatus.CurrentPosition = parameters.Position;

            return  Result<Process>.Ok(new Process(estimatedTime, estimatedTime));
        }

        public Result<Process> PickupTips(TipPickupCommand parameters)
        {
            Console.WriteLine("Picking-up tips from position: (" + 
                parameters.Position.X + ", " +
                parameters.Position.Y + ", " + 
                parameters.Position.Z + ")");

            //takes 3 seconds to pickup tips
            var estimatedTime = GetTravelTimeToPosition(parameters.Position) + new TimeSpan(0, 0, 3); 
            SimulateRuntimeWait(estimatedTime);

            _pipettorStatus.CurrentPosition = parameters.Position;

            return Result<Process>.Ok(new Process(estimatedTime, estimatedTime));
        }

        public Result<Process> DropTips(TipDropCommand parameters)
        {
            Console.WriteLine("Dropping tips into position: (" + 
                parameters.Position.X + ", " +
                parameters.Position.Y + ", " + 
                parameters.Position.Z + ")");

            //takes 3 seconds to drop tips
            var estimatedTime = GetTravelTimeToPosition(parameters.Position) + new TimeSpan(0, 0, 3); 
            SimulateRuntimeWait(estimatedTime);

            _pipettorStatus.CurrentPosition = parameters.Position;

            return Result<Process>.Ok(new Process(estimatedTime, estimatedTime));
        }

        
        public bool IsInitialized => throw new NotImplementedException();
        public Result<Process> Initialize()
        {
            throw new NotImplementedException();
        }

        public Result<Process> Deinitialize()
        {
            throw new NotImplementedException();
        }

        private TimeSpan GetTravelTimeToPosition(Coordinates position)
        {
            double distance = Math.Sqrt((_pipettorStatus.CurrentPosition.X - position.X)*(_pipettorStatus.CurrentPosition.X - position.X) + 
                (_pipettorStatus.CurrentPosition.Y - position.Y)*(_pipettorStatus.CurrentPosition.Y - position.Y) +
                (_pipettorStatus.CurrentPosition.Z - position.Z)*(_pipettorStatus.CurrentPosition.Z - position.Z));

            return new TimeSpan(0, 0, 0, 0, (int)(distance/_motorSpeed * 1000.0));
        }
        private void SimulateRuntimeWait(TimeSpan estimatedDuration)
        {
            if(SimulationSpeedFactor == 0)
                return;
            
            Thread.Sleep((int)estimatedDuration.TotalMilliseconds / (int)SimulationSpeedFactor);
        }

    }
}