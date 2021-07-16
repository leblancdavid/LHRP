using System;
using System.Text;
using System.Threading;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Devices.Pipettor
{
    public class DefaultSimulatedPipettor : IPipettor
    {
        private const double _motorSpeed = 100.0; //mm/s
        private const double _tipPickupFailureRate = 0.0;
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

        public IPipetteLogger Logger { get; private set; }

        public DefaultSimulatedPipettor(PipettorSpecification specification, IPipetteLogger? logger = null)
        {
            Specification = specification;
            NumberChannels = specification.NumChannels;
            PipettorStatus = new PipettorStatus(NumberChannels);
            SimulationSpeedFactor = 0;
            FailureRate = 0;
            if(logger == null)
            {
                Logger = new InMemoryPipetteLogger();
            }
            else
            {
                Logger = logger;
            }
        }

        public ProcessResult Aspirate(AspirateContext context)
        {
            var process = new ProcessResult();
            var targets = context.Targets;
            var sb = new StringBuilder();
            sb.Append("Aspirating with channels pattern '");
            sb.Append(targets.GetChannelString());
            sb.Append("' from: ");

            Coordinates position = new Coordinates();
            for (int i = 0; i < targets.NumChannels; ++i)
            {
                if (targets[i] != null)
                {
                    var target = targets[i];
                    sb.Append($"Pos{target!.Container.Address.InstanceId}-({target.Container.Address.ToAlphaAddress()}) {target.GetPipetteCoordinates()}, {target.Volume}uL, Liquid: {target.Liquid?.GetId()}; ");
                    process.Combine(PipettorStatus[i].OnAspiratedVolume(target.Container, target.Volume));

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

            Logger.LogTransfer(targets);

            return process;
        }

        public ProcessResult Dispense(DispenseContext context)
        {
            var process = new ProcessResult();
            var targets = context.Targets;
            var sb = new StringBuilder();
            sb.Append("Dispensing with channels pattern '");
            sb.Append(targets.GetChannelString());
            sb.Append("' to: ");

            Coordinates position = new Coordinates();
            for (int i = 0; i < targets.NumChannels; ++i)
            {
                if (targets[i] != null)
                {
                    var target = targets[i];
                    sb.Append($"Pos{target!.Container.Address.InstanceId}-({target.Container.Address.ToAlphaAddress()}) {target.GetPipetteCoordinates()}, {target.Volume}uL, Liquid: {PipettorStatus[i].CurrentLiquid?.GetId()}; ");
                    process.Combine(PipettorStatus[i].OnDispensedVolume(target.Container, target.Volume));
                }
                else
                {
                    sb.Append($"(*,*,*); ");
                }
            }

            //takes 3 seconds to pickup tips
            var estimatedTime = GetTravelTimeToPosition(position) + new TimeSpan(0, 0, 3);
            SimulateRuntimeWait(estimatedTime);

            PipettorStatus.CurrentPosition = position;

            Console.WriteLine(sb.ToString());

            Logger.LogTransfer(targets);

            return process;
        }

        public ProcessResult PickupTips(TipPickupParameters parameters)
        {
            var process = new ProcessResult();
            var sb = new StringBuilder();
            sb.Append("Picking-up tips with channels pattern '");
            sb.Append(parameters.Pattern.GetChannelString());
            sb.Append("' from: ");

            Coordinates position = new Coordinates();
            Random random = new Random();
            var errorPattern = ChannelPattern.Empty(parameters.Pattern.NumChannels);
            for (int i = 0; i < parameters.Pattern.NumChannels; ++i)
            {
                if (parameters.Pattern.IsInUse(i))
                {
                    var tip = parameters.Pattern.GetTip(i);
                    position = tip!.AbsolutePosition;
                    sb.Append($"Pos{tip.Address.InstanceId}-({tip.Address.ToAlphaAddress()}) {tip.AbsolutePosition}; ");
                    errorPattern.SetInUse(i, random.NextDouble() < _tipPickupFailureRate);
                    if (!errorPattern.IsInUse(i))
                    {
                        process.Combine(PipettorStatus[i].OnPickedUpTip(tip));
                    }
                }
                else
                {
                    sb.Append($"(*,*,*); ");
                }
            }

            //takes 3 seconds to pickup tips
            var estimatedTime = GetTravelTimeToPosition(position) + new TimeSpan(0, 0, 3);
            SimulateRuntimeWait(estimatedTime);

            PipettorStatus.CurrentPosition = position;

            Console.WriteLine(sb.ToString());

            Logger.BeginSequence(parameters.Pattern);

            return process;
        }

        public ProcessResult DropTips(TipDropParameters parameters)
        {
            var process = new ProcessResult();
            if (parameters.Pattern != null)
            {
                var sb = new StringBuilder();
                sb.Append("Dropping tips with channels pattern '");
                sb.Append(parameters.Pattern.GetChannelString());
                sb.Append("' to positions: ");

                Coordinates position = new Coordinates();
                for (int i = 0; i < parameters.Pattern.NumChannels; ++i)
                {
                    if (parameters.Pattern.IsInUse(i))
                    {
                        var tip = parameters.Pattern.GetTip(i);
                        position = tip!.AbsolutePosition;
                        sb.Append($"Pos{tip.Address.InstanceId}-({tip.Address.ToAlphaAddress()}) {tip.AbsolutePosition}; ");
                        process.Combine(PipettorStatus[i].OnDroppedTip());
                    }
                    else
                    {
                        sb.Append($"(*,*,*); ");
                    }
                }

                //takes 3 seconds to pickup tips
                var estimatedTime = GetTravelTimeToPosition(position) + new TimeSpan(0, 0, 3);
                SimulateRuntimeWait(estimatedTime);

                PipettorStatus.CurrentPosition = position;

                Console.WriteLine(sb.ToString());


                Logger.EndSequence(parameters.Pattern);

                return process;
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
                foreach (var status in PipettorStatus.ChannelStatus)
                {
                    process.Combine(status.OnDroppedTip());
                }

                Console.WriteLine(sb.ToString());

                return process;
            }
        }


        public bool IsInitialized => throw new NotImplementedException();


        public ProcessResult Initialize()
        {
            throw new NotImplementedException();
        }

        public ProcessResult Deinitialize()
        {
            throw new NotImplementedException();
        }

        private TimeSpan GetTravelTimeToPosition(Coordinates position)
        {
            double distance = Math.Sqrt((PipettorStatus.CurrentPosition!.X - position.X) * (PipettorStatus.CurrentPosition.X - position.X) +
                (PipettorStatus.CurrentPosition.Y - position.Y) * (PipettorStatus.CurrentPosition.Y - position.Y) +
                (PipettorStatus.CurrentPosition.Z - position.Z) * (PipettorStatus.CurrentPosition.Z - position.Z));

            return new TimeSpan(0, 0, 0, 0, (int)(distance / _motorSpeed * 1000.0));
        }
        private void SimulateRuntimeWait(TimeSpan estimatedDuration)
        {
            if (SimulationSpeedFactor == 0)
                return;

            Thread.Sleep((int)estimatedDuration.TotalMilliseconds / (int)SimulationSpeedFactor);
        }

        public IPipettor GetSimulation()
        {
            return this;
        }
    }
}