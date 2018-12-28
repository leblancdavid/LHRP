using System.Collections.Generic;
using LHRP.Api.Protocol;

namespace LHRP.Api.Runtime
{
    public interface IRuntimeQueue
    {
        IEnumerable<IRunnable> CommandQueue { get; }
        void Add(IRunnable command);
        void Insert(IRunnable command, int index);
        void Remove(int index);
        void Abort();


        
    }
}