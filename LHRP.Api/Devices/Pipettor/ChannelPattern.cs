using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    protected List<bool> _activeChannels;
    public bool this[int i]
    {
      get { return _activeChannels[i]; }
      set { _activeChannels[i] = value; }
    }

    public int GetNumberActiveChannels()
    {
      return _activeChannels.Where(a => a == true)
                    .Select(a => a)
                    .Count();
    }

    public string GetChannelString()
    {
      StringBuilder sb = new StringBuilder();
      foreach(var channelActive in _activeChannels)
      {
        if(channelActive)
        {
          sb.Append("1");
        }
        else
        {
          sb.Append("0");
        }
      }

      return sb.ToString();
    }

    public static ChannelPattern Empty(int numChannels)
    {
        return new ChannelPattern(numChannels);
    }

    public static ChannelPattern Full(int numChannels)
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