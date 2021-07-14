using System.Collections.Generic;
using CSharpFunctionalExtensions;
using LHRP.Api.Protocol;
using LHRP.Api.Runtime.Resources;

namespace LHRP.Api.Runtime
{
    public interface IRuntimeCommandQueue : ISnapshotCreator<IRuntimeCommandQueue>
    {
        IEnumerable<IRunnableCommand> Queue { get; }
        int CurrentCommandIndex { get; }
        IRunnableCommand? CurrentCommand { get; }
        bool IsCompleted { get; }

        IRunnableCommand? MoveToLastExecutedCommand();
        IRunnableCommand? Next();
        IRunnableCommand? Previous();
        IRunnableCommand? GetCommandAt(int index);
        ProcessResult RunNextCommand(IRuntimeEngine engine);

        ProcessResult RetryLastCommand(IRuntimeEngine engine);
        ProcessResult Clear();

        void Add(IRunnableCommand command);
        void Insert(int index, IRunnableCommand command);
        void Remove(int index);


        ResourcesUsage GetTotalResources();
        ResourcesUsage GetRemainingResources();



    }
}