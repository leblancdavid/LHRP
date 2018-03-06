using System.Collections.Generic;
using LHRP.Api.CoordinateSystem;
using LHRP.Api.Labware.Tips;

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
        public Tip CurrentTip { get; private set; }
        public double CurrentVolume { get; private set; }
        public bool HasErrors 
        { 
            get
            {
                return _errorMessages.Count > 0;
            }
        }

        private List<string> _errorMessages = new List<string>();
        public IEnumerable<string> ErrorMessages => _errorMessages;
        public Coordinates CurrentPosition { get; set; }

        public void AspiratedVolume(double volume)
        {
            if(!HasTip)
            {
                _errorMessages.Add($"Attempted to aspirate {volume}uL from a channel with no tip");
                return;
            }

            CurrentVolume += volume;
            if(CurrentVolume > CurrentTip.TipVolume)
            {
                _errorMessages.Add($"Current volume of {CurrentVolume} exceeds maximum tip capacity");
            }
        }

        public void DispensedVolume(double volume)
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

        public void PickedUpTip(Tip tip)
        {
            if(HasTip)
            {
                 _errorMessages.Add($"Attempted to pickup a tip on channel with no tip");
                return;
            }

            CurrentTip = tip;
        }

        public void DroppedTip()
        {
            CurrentTip = null;
            CurrentVolume = 0.0;
        }


    }
}