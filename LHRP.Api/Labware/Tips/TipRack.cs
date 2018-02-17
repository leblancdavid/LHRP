using System.Collections.Generic;
using System.Linq;
using LHRP.Api.Common;
using LHRP.Api.Devices;

namespace LHRP.Api.Labware.Tips
{
    public class TipRack : Labware
    {
        public int TotalTipCount 
        { 
            get
            {
                return Definition.Rows * Definition.Columns;
            }
        }
        public int RemainingTips 
        { 
            get
            {
                return _tips.Count;
            } 
        }
        
        public TipRackDefinition Definition { get; private set; }
        public override Coordinates AbsolutePosition 
        { 
            get
            {
                return _absolutePosition;
            } 
            set
            {
                //Before we update the absolute position, move all the tips along with the rack.
                foreach(var tip in _tips.Values)
                {
                    tip.AbsolutePosition.X = value.X - _absolutePosition.X;
                    tip.AbsolutePosition.Y = value.Y - _absolutePosition.Y;
                    tip.AbsolutePosition.Z = value.Z - _absolutePosition.Z;
                }
                _absolutePosition = value;
            }
        }

        private Dictionary<LabwareAddress, Tip> _tips = new Dictionary<LabwareAddress, Tip>();

        public TipRack(TipRackDefinition definition, Coordinates position, int positionId)
        {
            Definition = definition;
            UpdatePosition(position, PositionId);
            
            Refill();
        }

        
        public void Refill()
        {
            _tips.Clear();
            for(int i = 0; i < Definition.Rows; ++i)
            {
                for(int j = 0; j < Definition.Columns; ++j)
                {
                    var absolutePosition = new Coordinates()
                    {
                        X = AbsolutePosition.X + Definition.Offset.X + Definition.Spacing * j,
                        Y = AbsolutePosition.Y + Definition.Offset.Y + Definition.Spacing * i,
                        Z = AbsolutePosition.Z + Definition.Offset.Z
                    };

                    var labwareAddress = new LabwareAddress(i + 1, j + 1);
                    
                    _tips.Add(labwareAddress, new Tip(labwareAddress, absolutePosition, Definition.TipVolume, Definition.AreFilteredTips));
                }
            }
        }

        public Result Consume(LabwareAddress address)
        {
            if(!_tips.Remove(address))
            {
                return Result.Fail("Tip-rack does not contain a tip at address '" + address.ToString() + "'.");
            }

            return Result.Ok();
        }

        public Result<Tip> GetNextAvailableTip()
        {
            if(RemainingTips == 0)
            {
                return Result<Tip>.Fail("Tip-rack is empty.");
            }

            LabwareAddress nextAddress = _tips.Keys.FirstOrDefault();
            foreach(var address in _tips.Keys)
            {
                if(address.Column < nextAddress.Column)
                {
                    nextAddress = address;
                }
                else if(address.Column == nextAddress.Column && address.Row < nextAddress.Row)
                {
                    nextAddress = address;
                }
            }

            return Result<Tip>.Ok(_tips[nextAddress]);
        }

        public override Result<Coordinates> GetRealCoordinates(LabwareAddress address)
        {
            if(!_tips.ContainsKey(address))
            {
                return Result<Coordinates>.Fail("Invalid labware address");
            }

            return Result<Coordinates>.Ok(_tips[address].AbsolutePosition);
        }

    }
}