using System.Collections.Generic;
using LHRP.Api.Protocol;

namespace LHRP.Api.Runtime
{
    public interface IRuntimeCommandQueue
    {
        IEnumerable<IRunnableCommand> Queue { get; }
        int CurrentCommandIndex { get; }
        bool IsCompleted { get; }

        Process RunNextCommand(IRuntimeEngine engine);
        Process RetryLastCommand(IRuntimeEngine engine);
        Process Abort();

        void Add(IRunnableCommand command);
        void Insert(int index, IRunnableCommand command);
        void Remove(int index);


    }
}