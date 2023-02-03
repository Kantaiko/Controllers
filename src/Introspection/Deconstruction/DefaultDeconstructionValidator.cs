using System.Reflection;

namespace Kantaiko.Controllers.Introspection.Deconstruction;

/// <summary>
/// The default implementation of <see cref="IDeconstructionValidator" /> that
/// delegates the validation to the first attribute that implements <see cref="IDeconstructionValidator" />.
/// </summary>
public sealed class DefaultDeconstructionValidator : IDeconstructionValidator
{
    public bool CanDeconstruct(Type type, ICustomAttributeProvider attributeProvider)
    {
        var validator = type.GetCustomAttributes(true).OfType<IDeconstructionValidator>().FirstOrDefault();

        return validator?.CanDeconstruct(type, attributeProvider) ?? false;
    }
}
