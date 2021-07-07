using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Runtime.Scheduling
{
    public interface IScheduleStream
    {
        void Send(Schedule schedule);
    }
}
