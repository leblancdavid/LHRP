using System.Collections.Generic;
using LHRP.Api.Common;
using LHRP.Api.Devices;

namespace LHRP.Api.Labware.Tips
{
    public class TipRack
    {
        public int TotalTipCount 
        { 
            get
            {
                return Rows * Columns;
            }
        }
        public int RemainingTips 
        { 
            get
            {
                return _tips.Count;
            } 
        }
        public double TipVolume { get; private set; }
        public bool AreFilteredTips { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public Position Offset { get; private set; }
        public double Spacing { get; private set; }

        private Dictionary<LabwareAddress, Tip> _tips = new Dictionary<LabwareAddress, Tip>();

        public TipRack(int rows, int columns, double tipVolume, bool filtered,
            Position offset, double spacing)
        {
            Rows =  rows;
            Columns = columns;
            TipVolume = tipVolume;
            AreFilteredTips = filtered;
            Offset = offset;
            Spacing = spacing;
            Refill();
        }

        public void Refill()
        {
            _tips.Clear();
            for(int i = 0; i < Rows; ++i)
            {
                for(int j = 0; j < Columns; ++j)
                {
                    var labwareAddress = new LabwareAddress(i + 1, j + 1);
                    var relativePosition = new Position()
                    {
                        X = Offset.X + Spacing * j,
                        Y = Offset.Y + Spacing * i,
                        Z = Offset.Z
                    };
                    _tips.Add(labwareAddress, new Tip(labwareAddress, relativePosition, TipVolume, AreFilteredTips));
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