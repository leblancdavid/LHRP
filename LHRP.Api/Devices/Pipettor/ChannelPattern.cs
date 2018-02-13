using System.Collections.Generic;

namespace LHRP.Api.Devices.Pipettor
{
  public class ChannelPattern
  {
    public ChannelPattern(int numChannels)
    {
      this.NumChannels = numChannels;
      this._activeChannels = new List<bool>(numChannels);
    }
    public int NumChannels { get; private set; }

    private List<bool> _activeChannels;
    public bool this[int i]
    {
      get { return _activeChannels[i]; }
      set { _activeChannels[i] = value; }
    }

    public static ChannelPattern AllInactive(int numChannels)
    {
        return new ChannelPattern(numChannels);
    }

    public static ChannelPattern AllActive(int numChannels)
    {
        var cp = new ChannelPattern(numChannels);
        for(int i = 0; i < numChannels; ++i)
        {
            cp[i] = true;
        }
        return cp;
    }
  }
}