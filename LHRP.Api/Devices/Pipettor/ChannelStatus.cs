using System.Collections.Generic;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Labware;
using LHRP.Api.Liquids;

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
        public Liquid? CurrentLiquid { get; private set; }
        public bool HasErrors 
        { 
            get
            {
                return _errorMessages.Count > 0;
            }
        }

        private List<string> _errorMessages = new List<string>();
        public IEnumerable<string> ErrorMessages => _errorMessages;
        public Coordinates? CurrentPosition { get; set; }

        public void OnAspiratedVolume(Liquid liquid, double volume)
        {
            if(!HasTip)
            {
                _errorMessages.Add($"Attempted to aspirate {volume}uL from a channel with no tip");
                return;
            }

            CurrentVolume += volume;
            if(CurrentVolume > CurrentTip!.TipVolume)
            {
                _errorMessages.Add($"Current volume of {CurrentVolume} exceeds maximum tip capacity");
            }

            CurrentLiquid = liquid;
        }

        public void OnDispensedVolume(double volume)
        {
            if(!HasTip)
            {
                _errorMessages.Add($"Attempted to dispense {volume}uL from a channel with no tip");
                return;
            }

            CurrentVolume -= volume;
            if(CurrentVolume < 0)
            {
                CurrentVolume = 0.0;
            }
        }

        public void OnPickedUpTip(Tip tip)
        {
            if(HasTip)
            {
                 _errorMessages.Add($"Attempted to pickup a tip on channel with no tip");
                return;
            }

            CurrentTip = tip;
        }

        public void OnDroppedTip()
        {
            CurrentTip = null;
            CurrentVolume = 0.0;
        }


    }
}