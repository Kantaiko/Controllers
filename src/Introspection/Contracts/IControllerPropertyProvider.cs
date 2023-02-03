using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection.Contracts;

/// <summary>
/// The contract for a provider that can update controller properties.
/// </summary>
public interface IControllerPropertyProvider
{
    /// <summary>
    /// Updates the controller properties.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>A modified collection of controller properties or the same collection if no changes are made.</returns>
    IImmutablePropertyCollection UpdateControllerProperties(ControllerTransformationContext context);
}
