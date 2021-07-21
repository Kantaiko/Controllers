using System;

namespace Kantaiko.Controllers.Result
{
    public sealed class ExceptionExitReason : IExitReason
    {
        internal ExceptionExitReason(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}
