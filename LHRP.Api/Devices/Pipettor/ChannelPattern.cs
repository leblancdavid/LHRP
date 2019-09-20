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
            this._activeChannels = new bool[numChannels];
        }
        public int NumChannels { get; private set; }

        protected bool[] _activeChannels;
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

        public bool IsFull()
        {
            if(GetNumberActiveChannels() == NumChannels)
            return true;
            return false;
        }

        public bool IsEmpty()
        {
            if(GetNumberActiveChannels() == 0)
            return true;

            return false;
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

        public static ChannelPattern operator& (ChannelPattern b, ChannelPattern c)
        {
            int numChannels = b.NumChannels < c.NumChannels ? b.NumChannels : c.NumChannels;
            var a = new ChannelPattern(numChannels);
            for (int i = 0; i < numChannels; ++i)
            {
                a[i] = b[i] && c[i];
            }

            return a;
        }

        public static ChannelPattern operator| (ChannelPattern b, ChannelPattern c)
        {
            int numChannels = b.NumChannels < c.NumChannels ? b.NumChannels : c.NumChannels;
            var a = new ChannelPattern(numChannels);
            for (int i = 0; i < numChannels; ++i)
            {
                a[i] = b[i] || c[i];
            }

            return a;
        }

        public static ChannelPattern operator -(ChannelPattern b, ChannelPattern c)
        {
            int numChannels = b.NumChannels < c.NumChannels ? b.NumChannels : c.NumChannels;
            var a = new ChannelPattern(numChannels);
            for (int i = 0; i < numChannels; ++i)
            {
                a[i] = c[i] ? false : b[i];
            }

            return a;
        }

        public static ChannelPattern operator +(ChannelPattern b, ChannelPattern c)
        {
            int numChannels = b.NumChannels < c.NumChannels ? b.NumChannels : c.NumChannels;
            var a = new ChannelPattern(numChannels);
            for (int i = 0; i < numChannels; ++i)
            {
                a[i] = c[i] ? true : b[i];
            }

            return a;
        }
    }
}