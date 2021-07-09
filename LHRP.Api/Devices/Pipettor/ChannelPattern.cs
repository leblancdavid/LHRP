using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Devices.Pipettor
{
    public class ChannelPattern<T> where T : struct
    {
        public ChannelPattern(int numChannels)
        {
            NumChannels = numChannels;
            _channels = new T?[numChannels];
        }

        public int NumChannels { get; private set; }

        protected T?[] _channels;
        public T? this[int i]
        {
            get { return _channels[i]; }
            set { _channels[i] = value; }
        }

        public int GetNumberActiveChannels()
        {
            return _channels.Where(a => a != null)
                        .Select(a => a)
                        .Count();
        }

        public bool IsFull()
        {
            if (GetNumberActiveChannels() == NumChannels)
                return true;
            return false;
        }

        public bool IsEmpty()
        {
            if (GetNumberActiveChannels() == 0)
                return true;

            return false;
        }

        public string GetChannelString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var channelActive in _channels)
            {
                if (channelActive != null)
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

        public static ChannelPattern<T> Empty(int numChannels)
        {
            return new ChannelPattern<T>(numChannels);
        }

        public void Mask(ChannelPattern<bool> pattern)
        {
            for (int i = 0; i < NumChannels && i < pattern.NumChannels; ++i)
            {
                if(pattern[i] == false)
                {
                    _channels[i] = null;
                }
            }
        }

        //public static ChannelPattern Full(int numChannels)
        //{
        //    var cp = new ChannelPattern(numChannels);
        //    for (int i = 0; i < numChannels; ++i)
        //    {
        //        cp[i] = true;
        //    }
        //    return cp;
        //}

        //public static ChannelPattern operator &(ChannelPattern b, ChannelPattern c)
        //{
        //    int numChannels = b.NumChannels < c.NumChannels ? b.NumChannels : c.NumChannels;
        //    var a = new ChannelPattern(numChannels);
        //    for (int i = 0; i < numChannels; ++i)
        //    {
        //        a[i] = b[i] && c[i];
        //    }

        //    return a;
        //}

        //public static ChannelPattern operator |(ChannelPattern b, ChannelPattern c)
        //{
        //    int numChannels = b.NumChannels < c.NumChannels ? b.NumChannels : c.NumChannels;
        //    var a = new ChannelPattern(numChannels);
        //    for (int i = 0; i < numChannels; ++i)
        //    {
        //        a[i] = b[i] || c[i];
        //    }

        //    return a;
        //}

        //public static ChannelPattern operator -(ChannelPattern b, ChannelPattern c)
        //{
        //    int numChannels = b.NumChannels < c.NumChannels ? b.NumChannels : c.NumChannels;
        //    var a = new ChannelPattern(numChannels);
        //    for (int i = 0; i < numChannels; ++i)
        //    {
        //        a[i] = c[i] ? false : b[i];
        //    }

        //    return a;
        //}

        //public static ChannelPattern operator +(ChannelPattern b, ChannelPattern c)
        //{
        //    int numChannels = b.NumChannels < c.NumChannels ? b.NumChannels : c.NumChannels;
        //    var a = new ChannelPattern(numChannels);
        //    for (int i = 0; i < numChannels; ++i)
        //    {
        //        a[i] = c[i] ? true : b[i];
        //    }

        //    return a;
        //}
    }
}