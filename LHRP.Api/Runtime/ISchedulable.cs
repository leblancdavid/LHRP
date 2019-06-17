using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Runtime
{
    public interface ISchedulable
    {
        Schedule Schedule(IRuntimeEngine runtimeEngine);
    }
}
