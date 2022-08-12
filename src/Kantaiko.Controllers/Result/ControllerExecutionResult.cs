using System;
using System.Diagnostics.CodeAnalysis;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Result;

public class ControllerExecutionResult
{
    public bool IsMatched { get; private init; } = true;

    [MemberNotNullWhen(true, nameof(ExitReason))]
    public bool IsExited => ExitReason is not null;

    [MemberNotNullWhen(true, nameof(ReturnValue))]
    public bool HasReturnValue => ReturnValue is not null;

    public object? ReturnValue { get; private init; }
    public IExitReason? ExitReason { get; private init; }

    public static ControllerExecutionResult Success(object? result) => new() { ReturnValue = result };

    public static ControllerExecutionResult Empty { get; } = new();

    public static ControllerExecutionResult Error(string? errorMessage, EndpointParameterInfo? parameter = null) =>
        new()
        {
            ExitReason = new ErrorExitReason(errorMessage, parameter)
        };

    public static ControllerExecutionResult Exception(Exception exception) => new()
    {
        ExitReason = new ExceptionExitReason(exception)
    };

    public static ControllerExecutionResult NotMatched { get; } = new() { IsMatched = false };

    public static ControllerExecutionResult Interrupted { get; } = new() { ExitReason = new InterruptionExitReason() };
}
