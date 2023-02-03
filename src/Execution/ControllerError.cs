using Kantaiko.Controllers.Introspection;
using Kantaiko.Properties;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Execution;

/// <summary>
/// Represents a generic error that occurs during the controller execution.
/// <br/>
/// Contains information about exceptions in pipeline stages,
/// endpoint matchers, parameter converters and controller methods itself.
/// </summary>
public sealed record ControllerError : IReadOnlyPropertyContainer
{
    /// <summary>
    /// Creates a new instance of <see cref="ControllerError"/> with the specified <paramref name="code"/>.
    /// </summary>
    /// <param name="code">The error code.</param>
    public ControllerError(string code)
    {
        ArgumentException.ThrowIfNullOrEmpty(code);
        Code = code;
    }

    /// <summary>
    /// The string code that identifies the error.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// The optional message that describes the error.
    /// Should be used for debugging purposes only.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// The optional exception that caused the error.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// The optional endpoint that caused the error.
    /// For example, the endpoint that has thrown an exception.
    /// </summary>
    public EndpointInfo? Endpoint { get; init; }

    /// <summary>
    /// The optional parameter that caused the error.
    /// For example, the parameter that failed to convert.
    /// </summary>
    public EndpointParameterInfo? Parameter { get; init; }

    /// <summary>
    /// The optional inner error that caused the error and contains more information about its reason.
    /// </summary>
    public ControllerError? InnerError { get; init; }

    /// <summary>
    /// The immutable collection of user-defined properties associated with the error.
    /// </summary>
    public IReadOnlyPropertyCollection Properties { get; init; } = ImmutablePropertyCollection.Empty;
}
