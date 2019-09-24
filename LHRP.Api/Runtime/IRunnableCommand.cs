using System;

namespace LHRP.Api.Runtime
{
    public interface IRunnableCommand : IRunnable
    {
        Guid CommandId { get; }
        int RetryCount { get; }
    }
}