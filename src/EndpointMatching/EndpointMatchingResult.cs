using Kantaiko.Controllers.Execution;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.EndpointMatching;

/// <summary>
/// Represents the result of matching an endpoint.
/// </summary>
public readonly struct EndpointMatchingResult
{
    /// <summary>
    /// Creates a new instance of <see cref="EndpointMatchingResult"/>.
    /// </summary>
    /// <param name="matched">Whether the endpoint was matched.</param>
    /// <param name="error">The error that caused the endpoint to not match.</param>
    public EndpointMatchingResult(bool matched, ControllerError? error = null)
    {
        Matched = matched;
        Error = error;
    }

    /// <summary>
    /// Whether the endpoint was matched.
    /// </summary>
    public bool Matched { get; }

    /// <summary>
    /// The error that occurred during matching the endpoint if any.
    /// </summary>
    public ControllerError? Error { get; }

    /// <summary>
    /// The collection of user-defined properties associated with the endpoint matching result.
    /// </summary>
    public IImmutablePropertyCollection MatchProperties { get; init; } = ImmutablePropertyCollection.Empty;

    /// <summary>
    /// Creates a matching result from the boolean value.
    /// </summary>
    /// <param name="matched">Whether the endpoint was matched.</param>
    /// <returns>The matching result.</returns>
    public static implicit operator EndpointMatchingResult(bool matched) => new(matched);

    /// <summary>
    /// Creates a matching result from the <see cref="ControllerError"/>.
    /// </summary>
    /// <param name="error">The error that caused the endpoint to not match.</param>
    /// <returns>The matching result.</returns>
    public static implicit operator EndpointMatchingResult(ControllerError error) => new(false, error);
}
