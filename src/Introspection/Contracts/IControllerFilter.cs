using Kantaiko.Controllers.Introspection.Context;

namespace Kantaiko.Controllers.Introspection.Contracts;

/// <summary>
/// The contract for controller filters.
/// </summary>
public interface IControllerFilter
{
    /// <summary>
    /// Decides whether the controller should be included in the controller tree.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>true if the controller should be included; otherwise, false.</returns>
    bool IsIncluded(ControllerTransformationContext context);
}
