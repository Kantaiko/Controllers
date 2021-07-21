using System;

namespace Kantaiko.Controllers.Internal
{
    internal readonly struct InvocationResult
    {
        public InvocationResult(object? result = null, Exception? exception = null)
        {
            Result = result;
            Exception = exception;
        }

        public object? Result { get; }
        public Exception? Exception { get; }
    }
}
