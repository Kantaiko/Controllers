using Kantaiko.Controllers.Introspection.Context;

namespace Kantaiko.Controllers.Introspection.Contracts;

/// <summary>
/// Provides a mechanism for customizing a parameter name, optionality and default value.
/// </summary>
public interface IParameterCustomizationProvider
{
    /// <summary>
    /// Returns a parameter name and optionality.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>A model of a parameter customization.</returns>
    ParameterCustomization GetParameterCustomization(ParameterTransformationContext context);
}
