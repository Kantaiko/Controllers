using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection.Contracts;

/// <summary>
/// The contract for a provider that can update endpoint properties.
/// </summary>
public interface IEndpointPropertyProvider
{
    /// <summary>
    /// Updates the endpoint properties.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>A modified collection of endpoint properties or the same collection if no changes are made.</returns>
    IImmutablePropertyCollection UpdateEndpointProperties(EndpointTransformationContext context);
}
