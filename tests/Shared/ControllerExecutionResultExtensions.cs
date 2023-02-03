using Kantaiko.Controllers.Execution;

namespace Kantaiko.Controllers.Tests.Shared;

internal static class ControllerExecutionResultExtensions
{
    public static void ThrowOnError(this ControllerExecutionResult result)
    {
        if (result.Error is not { } error) return;

        var exception = error.Exception;

        if (exception is null && error.InnerError is { } innerError)
        {
            exception = new InvalidOperationException($"[{innerError.Code}] {innerError.Message}");
        }

        throw new InvalidOperationException($"[{error.Code}] {error.Message}", exception);
    }
}
