using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Protocol;

namespace LHRP.Api.Runtime
{
    public interface IRuntimeCommandQueue
    {
        IEnumerable<IRunnableCommand> Queue { get; }
        int CurrentCommandIndex { get; }
        IRunnableCommand CurrentCommand { get; }
        bool IsCompleted { get; }

        void MoveToLastExecutedCommand();
        ProcessResult RunNextCommand(IRuntimeEngine engine);
        ProcessResult RetryLastCommand(IRuntimeEngine engine);
        ProcessResult Clear();

        void Add(IRunnableCommand command);
        void Insert(int index, IRunnableCommand command);
        void Remove(int index);

        Result<IRunnableCommand> GetCommandAt(int index); 


    }
}