using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Result;

public sealed class ErrorExitReason : IExitReason
{
    internal ErrorExitReason(string? errorMessage, EndpointParameterInfo? parameter)
    {
        ErrorMessage = errorMessage;
        Parameter = parameter;
    }

    public string? ErrorMessage { get; }
    public EndpointParameterInfo? Parameter { get; }
}
