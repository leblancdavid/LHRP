using System.Collections.Generic;
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
        public Position AbsolutePosition { get; private set; }
        private Dictionary<LabwareAddress, Tip> _tips = new Dictionary<LabwareAddress, Tip>();

        public TipRack(TipRackDefinition definition, Position position)
        {
            Definition = definition;
            AssignToPosition(position);
        }

        public void AssignToPosition(Position position)
        {
            AbsolutePosition = position;
            Refill();
        }

        public void Refill()
        {
            _tips.Clear();
            for(int i = 0; i < Definition.Rows; ++i)
            {
                for(int j = 0; j < Definition.Columns; ++j)
                {
                    var labwareAddress = new LabwareAddress(i + 1, j + 1);
                    var absolutePosition = new Position()
                    {
                        X = AbsolutePosition.X + Definition.Offset.X + Definition.Spacing * j,
                        Y = AbsolutePosition.Y + Definition.Offset.Y + Definition.Spacing * i,
                        Z = AbsolutePosition.Z + Definition.Offset.Z
                    };
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

        public Result<LabwareAddress> GetNextAvailableTip()
        {
            if(RemainingTips == 0)
            {
                return Result<LabwareAddress>.Fail("Tip-rack is empty.");
            }

            var nextAddress = new LabwareAddress(int.MaxValue, int.MaxValue);
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

            return Result<LabwareAddress>.Ok(nextAddress);
        }

    }
}