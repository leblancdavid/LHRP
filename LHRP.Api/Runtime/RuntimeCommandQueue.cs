using System.Collections.Generic;

namespace LHRP.Api.Runtime
{
    public class RuntimeCommandQueue : IRuntimeCommandQueue
    {
        public IEnumerable<IRunnableCommand> Queue => throw new System.NotImplementedException();

        public int CurrentCommandIndex => throw new System.NotImplementedException();

        public bool IsCompleted => throw new System.NotImplementedException();

        public Process Abort()
        {
            throw new System.NotImplementedException();
        }

        public void Add(IRunnableCommand command)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(IRunnableCommand command, int index)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(int index)
        {
            throw new System.NotImplementedException();
        }

        public Process RetryLastCommand()
        {
            throw new System.NotImplementedException();
        }

        public Process RunNextCommand()
        {
            throw new System.NotImplementedException();
        }
    }
}