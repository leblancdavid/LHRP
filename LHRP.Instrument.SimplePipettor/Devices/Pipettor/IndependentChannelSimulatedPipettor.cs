using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CSharpFunctionalExtensions;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Runtime;

namespace LHRP.Instrument.SimplePipettor.Devices.Pipettor
{
    public class IndependentChannelSimulatedPipettor: IPipettor, ISimulation
    {
        private const double _motorSpeed = 100.0; //mm/s
        public uint SimulationSpeedFactor { get; set; }
        public double FailureRate { get; set; }
        public int NumberChannels { get; private set; }
        public PipettorStatus PipettorStatus { get; private set; }
        public IDeviceStatus DeviceStatus 
        { 
            get { return PipettorStatus; } 
        }

        public Guid DeviceId { get; private set; }

        public PipettorSpecification Specification { get; private set; }


        public IndependentChannelSimulatedPipettor()
        {
            NumberChannels = 2;
            PipettorStatus = new PipettorStatus(NumberChannels);
            SimulationSpeedFactor = 1;
            FailureRate = 0;

            var channelSpecification = new List<ChannelSpecification>();
            //Add two channels that can reach anywhere on the deck for now.
            channelSpecification.Add(new ChannelSpecification(
                new Coordinates(double.MinValue, double.MinValue, double.MinValue),
                new Coordinates(double.MaxValue, double.MaxValue, double.MaxValue)
            )); 
            channelSpecification.Add(new ChannelSpecification(
                new Coordinates(double.MinValue, double.MinValue, double.MinValue),
                new Coordinates(double.MaxValue, double.MaxValue, double.MaxValue)
            ));

            Specification = new PipettorSpecification(channelSpecification,
                new Coordinates(0.0, 9.0, 0.0),
                true); 
        }

        public Process Aspirate(AspirateParameters parameters)
        {
            // Console.WriteLine("Aspirating " + parameters.Volume + "uL from position: (" + 
            //     parameters.Position.X + ", " +
            //     parameters.Position.Y + ", " + 
            //     parameters.Position.Z + ")");

            // var estimatedTime = GetTravelTimeToPosition(parameters.Position) + new TimeSpan(0, 0, 1);
            // SimulateRuntimeWait(estimatedTime);

            // _pipettorStatus.CurrentPosition = parameters.Position;

            // return new Process(estimatedTime, estimatedTime);
            return new Process();
        }

        public Process Dispense(DispenseParameters parameters)
        {
            // Console.WriteLine("Dispensing " + parameters.Volume + "uL to position: (" + 
            //     parameters.Position.X + ", " +
            //     parameters.Position.Y + ", " + 
            //     parameters.Position.Z + ")");

            // var estimatedTime = GetTravelTimeToPosition(parameters.Position) + new TimeSpan(0, 0, 1);
            // SimulateRuntimeWait(estimatedTime);

            // _pipettorStatus.CurrentPosition = parameters.Position;

            // return new Process(estimatedTime, estimatedTime);
            return  new Process();
        }

        public Process PickupTips(TipPickupParameters parameters)
        {
            var sb = new StringBuilder();
            sb.Append("Picking-up tips with channels pattern '");
            sb.Append(parameters.Pattern.GetChannelString());
            sb.Append("' from: ");

            Coordinates position = new Coordinates();
            for(int i = 0; i < parameters.Pattern.NumChannels; ++i)
            {
                if(parameters.Pattern[i])
                {
                    var tip = parameters.Pattern.GetTip(i);
                    position = tip.AbsolutePosition;
                    sb.Append($"Pos{tip.Address.PositionId}-({tip.Address.Row},{tip.Address.Column});");
                    PipettorStatus[i].OnPickedUpTip(tip);
                }
                else
                {
                    sb.Append($"(*,*,*);");
                }
            }

            //takes 3 seconds to pickup tips
            var estimatedTime = GetTravelTimeToPosition(position) + new TimeSpan(0, 0, 3); 
            SimulateRuntimeWait(estimatedTime);

            PipettorStatus.CurrentPosition = position;

            Console.WriteLine(sb.ToString());

            return new Process(estimatedTime, estimatedTime);
        }

        public Process DropTips(TipDropParameters parameters)
        {
            if(parameters.Pattern != null)
            {
                var sb = new StringBuilder();
                sb.Append("Dropping tips with channels pattern '");
                sb.Append(parameters.Pattern.GetChannelString());
                sb.Append("' to positions: ");

                Coordinates position = new Coordinates();
                for(int i = 0; i < parameters.Pattern.NumChannels; ++i)
                {
                    if(parameters.Pattern[i])
                    {
                        var tip = parameters.Pattern.GetTip(i);
                        position = tip.AbsolutePosition;
                        sb.Append($"Pos{tip.Address.PositionId}-({tip.Address.Row},{tip.Address.Column});");
                        PipettorStatus[i].OnDroppedTip();
                    }
                    else
                    {
                        sb.Append($"(*,*,*);");
                    }
                }

                //takes 3 seconds to pickup tips
                var estimatedTime = GetTravelTimeToPosition(position) + new TimeSpan(0, 0, 3); 
                SimulateRuntimeWait(estimatedTime);

                PipettorStatus.CurrentPosition = position;

                Console.WriteLine(sb.ToString());

                return new Process(estimatedTime, estimatedTime);
            }
            else
            {
                 var sb = new StringBuilder();
                sb.Append("Dropping tips to waste");

                Coordinates position = new Coordinates();
               
                //takes 3 seconds to pickup tips
                var estimatedTime = GetTravelTimeToPosition(position) + new TimeSpan(0, 0, 3); 
                SimulateRuntimeWait(estimatedTime);

                PipettorStatus.CurrentPosition = position;

                Console.WriteLine(sb.ToString());

                return new Process(estimatedTime, estimatedTime);
            }
        }

        
        public bool IsInitialized => throw new NotImplementedException();
        public Process Initialize()
        {
            throw new NotImplementedException();
        }

        public Process Deinitialize()
        {
            throw new NotImplementedException();
        }

        private TimeSpan GetTravelTimeToPosition(Coordinates position)
        {
            double distance = Math.Sqrt((PipettorStatus.CurrentPosition.X - position.X)*(PipettorStatus.CurrentPosition.X - position.X) + 
                (PipettorStatus.CurrentPosition.Y - position.Y)*(PipettorStatus.CurrentPosition.Y - position.Y) +
                (PipettorStatus.CurrentPosition.Z - position.Z)*(PipettorStatus.CurrentPosition.Z - position.Z));

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