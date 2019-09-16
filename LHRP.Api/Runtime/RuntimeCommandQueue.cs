using System.Collections.Generic;

namespace LHRP.Api.Runtime
{
    public class RuntimeCommandQueue : IRuntimeCommandQueue
    {
        private List<IRunnableCommand> _queue;
        public IEnumerable<IRunnableCommand> Queue => _queue;
        public int CurrentCommandIndex { get; private set; }
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

        public void MoveToLastCommand()
        {
            if(CurrentCommandIndex > 0)
            {
                CurrentCommandIndex--;
            }
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
    }
}