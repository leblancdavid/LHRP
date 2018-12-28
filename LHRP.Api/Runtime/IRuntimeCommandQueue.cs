using System.Collections.Generic;
using LHRP.Api.Protocol;

namespace LHRP.Api.Runtime
{
    public interface IRuntimeCommandQueue
    {
        IEnumerable<IRunnableCommand> Queue { get; }
        int CurrentCommandIndex { get; }
        bool IsCompleted { get; }

        Process RunNextCommand();
        Process RetryLastCommand();
        Process Abort();

        void Add(IRunnableCommand command);
        void Insert(IRunnableCommand command, int index);
        void Remove(int index);


    }
}