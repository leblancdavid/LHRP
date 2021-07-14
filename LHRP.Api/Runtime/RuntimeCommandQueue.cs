using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LHRP.Api.Runtime.Resources;

namespace LHRP.Api.Runtime
{
    public class RuntimeCommandQueue : IRuntimeCommandQueue
    {
        private List<IRunnableCommand> _queue;
        public IEnumerable<IRunnableCommand> Queue => _queue;
        public int CurrentCommandIndex { get; private set; }
        public IRunnableCommand? CurrentCommand
        {
            get
            {
                if(IsCompleted)
                {
                    return null;
                }
                return _queue[CurrentCommandIndex];
            }
        }
        public bool IsCompleted => CurrentCommandIndex == _queue.Count;

        public RuntimeCommandQueue()
        {
            _queue = new List<IRunnableCommand>();
            CurrentCommandIndex = 0;
        }

        public ProcessResult Clear()
        {
            CurrentCommandIndex = 0;
            _queue.Clear();
            return new ProcessResult();
        }

        public void Add(IRunnableCommand command)
        {
            _queue.Add(command);
        }

        public void Insert(int index, IRunnableCommand command)
        {
            _queue.Insert(index, command);
        }

        public void Remove(int index)
        {
            _queue.RemoveAt(index);
        }

        public IRunnableCommand? MoveToLastExecutedCommand()
        {
            if(CurrentCommandIndex > 0)
            {
                CurrentCommandIndex--;
            }

            return CurrentCommand;
        }

        public void MoveTo(int commandIndex)
        {
            if (commandIndex < 0 || commandIndex >= _queue.Count())
            {
                return;
            }

            CurrentCommandIndex = commandIndex;
        }

        public ProcessResult RetryLastCommand(IRuntimeEngine engine)
        {
            if(IsCompleted)
            {
                return new ProcessResult();
            }

            return _queue[CurrentCommandIndex].Run(engine);
        }

        public ProcessResult RunNextCommand(IRuntimeEngine engine)
        {
            if(IsCompleted)
            {
                return new ProcessResult();
            }

            var result = _queue[CurrentCommandIndex].Run(engine);

            CurrentCommandIndex++;
            return result;
        }

        public IRunnableCommand? GetCommandAt(int index)
        {
            if(index < 0 || index >= _queue.Count)
            {
                return null;
            }

            return _queue[index];
        }

        public ResourcesUsage GetTotalResources()
        {
            var resources = new ResourcesUsage();
            resources.Combine(_queue.Select(x => x.ResourcesUsed).ToArray());
            return resources;
        }

        public ResourcesUsage GetRemainingResources()
        {
            var resources = new ResourcesUsage();
            resources.Combine(_queue.Skip(CurrentCommandIndex).Select(x => x.ResourcesUsed).ToArray());
            return resources;
        }

        public IRuntimeCommandQueue CreateSnapshot()
        {
            var queue = new RuntimeCommandQueue();
            foreach(var command in _queue)
            {
                queue.Add(command);
            }

            queue.MoveTo(CurrentCommandIndex);

            return queue;
        }

        public IRunnableCommand? Next()
        {
            if (IsCompleted)
            {
                return null;
            }

            CurrentCommandIndex++;
            return CurrentCommand;
        }

        public IRunnableCommand? Previous()
        {
            if (CurrentCommandIndex > 0)
            {
                CurrentCommandIndex--;
            }

            return CurrentCommand;
        }
    }
}