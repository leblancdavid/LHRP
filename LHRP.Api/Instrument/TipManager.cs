using System.Collections.Generic;
using LHRP.Api.Common;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware.Tips;

namespace LHRP.Api.Instrument
{
    public class TipManager : ITipManager
    {
        private Dictionary<int, TipRack> _tipRacks = new Dictionary<int, TipRack>();
        public TipManager()
        {

        }

        public Result AssignTipRack(int positionId, TipRack tipRack)
        {
            
            throw new System.NotImplementedException();
        }

        public Result RequestTips(ChannelPattern pattern)
        {
            throw new System.NotImplementedException();
        }
    }
}