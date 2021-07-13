using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Devices.Pipettor
{
    public class ChannelPattern
    {
        public int NumChannels { get; private set; }
        protected bool[] _active;

        public ChannelPattern(int numChannels)
        {
            NumChannels = numChannels;
            _active = new bool[numChannels];
        }

        public ChannelPattern(string pattern)
        {
            NumChannels = pattern.Length;
            _active = pattern.Select(x => x == '1').ToArray();
        }

        public bool IsInUse(int index)
        {
            if(index < 0 || index >= NumChannels)
            {
                return false;
            }
            return _active[index];
        }

        public void SetInUse(int index, bool active)
        {
            if (index >= 0 && index < NumChannels)
            {
                _active[index] = active;
            }
        }

        public int GetNumberActiveChannels()
        {
            return _active.Where(a => a)
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
            foreach (var channelActive in _active)
            {
                if (channelActive)
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
            for (int i = 0; i < numChannels; ++i)
            {
                cp.SetInUse(i, true);
            }
            return cp;
        }


        public void Mask(ChannelPattern pattern)
        {
            for (int i = 0; i < NumChannels && i < pattern.NumChannels; ++i)
            {
                if (!pattern.IsInUse(i))
                {
                    _active[i] = false;
                }
            }
        }

        public static ChannelPattern operator &(ChannelPattern b, ChannelPattern c)
        {
            int numChannels = b.NumChannels < c.NumChannels ? b.NumChannels : c.NumChannels;
            var a = new ChannelPattern(numChannels);
            for (int i = 0; i < numChannels; ++i)
            {
                a.SetInUse(i, b.IsInUse(i) && c.IsInUse(i));
            }

            return a;
        }
    }

    public class ChannelPattern<T> : ChannelPattern where T : class?
    {
        public ChannelPattern(int numChannels)
            :base(numChannels)
        {
            _channels = new T?[numChannels];
        }

        public ChannelPattern(T?[] channels)
           : base(channels.Length)
        {
            _channels = channels;
        }

        protected T?[] _channels;
        public T? this[int i]
        {
            get 
            {
                if (i < 0 || i >= NumChannels)
                    return null;

                return _channels[i]; 
            }
            set
            {
                if (i < 0 || i >= NumChannels)
                    return;

                _channels[i] = value;
                _active[i] = value != null;               
            }
        }

        public IEnumerable<T> GetActiveChannels()
        {
            return _channels.Where(x => x != null).Select(x => x!);
        }

        public static ChannelPattern<T> operator -(ChannelPattern<T> b, ChannelPattern c)
        {
            int numChannels = b.NumChannels < c.NumChannels ? b.NumChannels : c.NumChannels;
            var a = new ChannelPattern<T>(numChannels);
            for (int i = 0; i < numChannels; ++i)
            {
                a[i] = c.IsInUse(i) ? null : b[i];
            }

            return a;
        }
    }
}