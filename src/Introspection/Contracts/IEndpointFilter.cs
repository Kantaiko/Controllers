using Kantaiko.Controllers.Introspection.Context;

namespace Kantaiko.Controllers.Introspection.Contracts;

/// <summary>
/// The contract for endpoint filters.
/// </summary>
public interface IEndpointFilter
{
    /// <summary>
    /// Decides whether the endpoint should be included in the endpoint list.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>true if the endpoint should be included; otherwise, false.</returns>
    bool IsIncluded(EndpointTransformationContext context);
}
