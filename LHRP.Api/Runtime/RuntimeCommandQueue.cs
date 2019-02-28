using System.Collections.Generic;

namespace LHRP.Api.Runtime
{
    public class RuntimeCommandQueue : IRuntimeCommandQueue
    {
        private List<IRunnableCommand> _queue;
        public IEnumerable<IRunnableCommand> Queue => _queue;
        public int CurrentCommandIndex { get; private set; }
        public bool IsCompleted => CurrentCommandIndex >= _queue.Count;

        public RuntimeCommandQueue()
        {
            _queue = new List<IRunnableCommand>();
            CurrentCommandIndex = -1;
        }

        public Process Abort()
        {
            throw new System.NotImplementedException();
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

        public Process RetryLastCommand(IRuntimeEngine engine)
        {
            if(IsCompleted)
            {
                return new Process();
            }

            return _queue[CurrentCommandIndex].Run(engine);
        }

        public Process RunNextCommand(IRuntimeEngine engine)
        {
            CurrentCommandIndex++;
            if(IsCompleted)
            {
                return new Process();
            }

            return _queue[CurrentCommandIndex].Run(engine);
        }
    }
}