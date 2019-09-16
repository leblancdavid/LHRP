using System.Collections.Generic;
using LHRP.Api.Protocol;

namespace LHRP.Api.Runtime
{
    public interface IRuntimeCommandQueue
    {
        IEnumerable<IRunnableCommand> Queue { get; }
        int CurrentCommandIndex { get; }
        bool IsCompleted { get; }

        void MoveToLastCommand();
        ProcessResult RunNextCommand(IRuntimeEngine engine);
        ProcessResult RetryLastCommand(IRuntimeEngine engine);
        ProcessResult Clear();

        void Add(IRunnableCommand command);
        void Insert(int index, IRunnableCommand command);
        void Remove(int index);


    }
}