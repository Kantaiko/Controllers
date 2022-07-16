using System;
using System.Diagnostics.CodeAnalysis;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Result;

public class ControllerResult
{
    public bool IsMatched { get; private init; } = true;

    [MemberNotNullWhen(true, nameof(ExitReason))]
    public bool IsExited => ExitReason is not null;

    [MemberNotNullWhen(true, nameof(ReturnValue))]
    public bool HasReturnValue => ReturnValue is not null;

    public object? ReturnValue { get; private init; }
    public IExitReason? ExitReason { get; private init; }

    public static ControllerResult Success(object? result) => new() { ReturnValue = result };

    public static ControllerResult Empty { get; } = new();

    public static ControllerResult Error(string? errorMessage, EndpointParameterInfo? parameter = null) => new()
    {
        ExitReason = new ErrorExitReason(errorMessage, parameter)
    };

    public static ControllerResult Exception(Exception exception) => new()
    {
        ExitReason = new ExceptionExitReason(exception)
    };

    public static ControllerResult NotMatched { get; } = new() { IsMatched = false };

    public static ControllerResult Interrupted { get; } = new() { ExitReason = new InterruptionExitReason() };
}
