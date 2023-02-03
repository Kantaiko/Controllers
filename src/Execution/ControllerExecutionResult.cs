using System.Diagnostics.CodeAnalysis;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.Execution;

/// <summary>
/// Represents a result of a controller execution.
/// May contain an error or an optional value returned by an executed endpoint.
/// </summary>
public sealed record ControllerExecutionResult : IReadOnlyPropertyContainer
{
    /// <summary>
    /// Indicates whether the execution was successful.
    /// <br/>
    /// The success of the execution does not mean that the result will contain a value.
    /// An executed endpoint may not return a value at all.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success => Error is null;

    /// <summary>
    /// The optional value returned by an executed endpoint.
    /// </summary>
    public object? EndpointResult { get; init; }

    /// <summary>
    /// The error that occurred during the execution if it was unsuccessful.
    /// </summary>
    public ControllerError? Error { get; init; }

    /// <summary>
    /// The immutable collection of user-defined properties associated with the execution result.
    /// </summary>
    public IReadOnlyPropertyCollection Properties { get; } = new PropertyCollection();
}
