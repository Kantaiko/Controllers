using Kantaiko.Properties;

namespace Kantaiko.Controllers.EndpointMatching;

/// <summary>
/// Endpoint properties that contains the list of endpoint matchers.
/// </summary>
public sealed record EndpointMatchingProperties(IReadOnlyList<IEndpointMatcher> Matchers) :
    PropertyRecord<EndpointMatchingProperties>;
