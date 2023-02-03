using System.Reflection;

namespace Kantaiko.Controllers.Introspection.Deconstruction;

/// <summary>
/// Defines a validator that determines whether a type can be deconstructed into multiple parameters.
/// </summary>
public interface IDeconstructionValidator
{
    /// <summary>
    /// Determines whether a type can be deconstructed into multiple parameters.
    /// </summary>
    /// <param name="type">The type to deconstruct.</param>
    /// <param name="attributeProvider">The attribute provider to check for attributes.</param>
    /// <returns>Whether the type can be deconstructed.</returns>
    bool CanDeconstruct(Type type, ICustomAttributeProvider attributeProvider);
}
