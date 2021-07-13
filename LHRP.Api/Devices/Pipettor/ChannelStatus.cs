using System.Collections.Generic;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;

namespace LHRP.Api.Devices.Pipettor
{
    public class ChannelStatus : IDeviceStatus
    {
        public bool HasTip
        {
            get
            {
                return CurrentTip != null;
            }
        }

        public bool ContainsLiquid
        {
            get
            {
                return CurrentLiquid != null && CurrentVolume > 0.0;
            }
        }

        public Tip? CurrentTip { get; private set; }
        public double CurrentVolume { get; private set; }
        public HeterogeneousLiquid? CurrentLiquid { get; private set; }
        public bool HasErrors
        {
            get
            {
                return _errorMessages.Count > 0;
            }
        }

        public double EmptyVolume
        {
            get
            {
                if (CurrentTip == null)
                    return 0.0;

                return CurrentTip.TipVolume - CurrentVolume;
            }
        }

        private List<string> _errorMessages = new List<string>();
        public IEnumerable<string> ErrorMessages => _errorMessages;
        public Coordinates? CurrentPosition { get; set; }

        public bool CanAspirate(double volume)
        {
            if (!HasTip || EmptyVolume < volume)
            {
                return false;
            }

            return true;
        }

        public bool CanDispense(double volume)
        {
            if (!HasTip || CurrentVolume < volume)
            {
                return false;
            }

            return true;
        }

        public ProcessResult OnAspiratedVolume(Liquid liquid, double volume)
        {
            var process = new ProcessResult();
            if (!HasTip)
            {
                var msg = $"Attempted to aspirate {volume}uL from a channel with no tip";
                process.AddError(new RuntimeError(msg));
                _errorMessages.Add(msg);
                return process;
            }

            if (!CanAspirate(volume))
            {
                var msg = $"Tip volume exceeded: Attempted to aspirate {volume}uL from a channel with {EmptyVolume}uL of available volume";
                process.AddError(new RuntimeError(msg));
                _errorMessages.Add(msg);
                return process;
            }

            if(CurrentLiquid == null)
            {
                CurrentLiquid = new HeterogeneousLiquid();
            }
            CurrentLiquid.Mix(liquid, volume / (CurrentVolume + volume));
            CurrentVolume += volume;
            return process;
        }

        public ProcessResult OnDispensedVolume(double volume)
        {
            var process = new ProcessResult();
            if (!HasTip)
            {
                var msg = $"Attempted to dispense {volume}uL from a channel with no tip";
                process.AddError(new RuntimeError(msg));
                _errorMessages.Add(msg);
                return process;
            }

            CurrentVolume -= volume;
            if (CurrentVolume < 0)
            {
                CurrentVolume = 0.0;
            }

            return process;
        }

        public ProcessResult OnPickedUpTip(Tip tip)
        {
            var process = new ProcessResult();
            if (HasTip)
            {
                var msg = $"Attempted to pickup a tip on channel with a tip already";
                process.AddError(new RuntimeError(msg));
                _errorMessages.Add(msg);
                return process;
            }

            CurrentTip = tip;
            return process;
        }

        public ProcessResult OnDroppedTip()
        {
            CurrentTip = null;
            CurrentVolume = 0.0;
            CurrentLiquid = null;

            return new ProcessResult();
        }


    }
}