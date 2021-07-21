using System;
using System.Diagnostics.CodeAnalysis;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Middleware;

namespace Kantaiko.Controllers.Result
{
    public class RequestProcessingResult
    {
        public bool IsMatched { get; private init; } = true;

        [MemberNotNullWhen(true, nameof(ExitReason))]
        public bool IsExited => ExitReason is not null;

        [MemberNotNullWhen(true, nameof(ReturnValue))]
        public bool HasReturnValue => ReturnValue is not null;

        public object? ReturnValue { get; private init; }
        public IExitReason? ExitReason { get; private set; }

        internal static RequestProcessingResult Success(object? result) => new() {ReturnValue = result};

        internal static RequestProcessingResult Empty { get; } = new();

        internal static RequestProcessingResult Error(RequestErrorStage errorStage, string errorMessage,
            EndpointParameterInfo? parameter) => new()
        {
            ExitReason = new ErrorExitReason(errorStage, errorMessage, parameter)
        };

        internal static RequestProcessingResult Exception(Exception exception) => new()
        {
            ExitReason = new ExceptionExitReason(exception)
        };

        internal static RequestProcessingResult NotMatched { get; } = new() {IsMatched = false};

        internal static RequestProcessingResult Interrupted(EndpointMiddlewareStage middlewareStage) => new()
        {
            ExitReason = new InterruptionExitReason(middlewareStage)
        };
    }
}
