using System.Reflection;
using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.Exceptions;

/// <summary>
/// The exception that is thrown when a composite parameter contains two composite properties with the same type.
/// </summary>
public sealed class DuplicateCompositePropertyException : Exception
{
    internal DuplicateCompositePropertyException(PropertyInfo first, PropertyInfo second) : base(
        string.Format(Strings.DuplicateCompositeProperty,
            first.DeclaringType!.Name,
            first.PropertyType,
            first.Name,
            second.Name)
    ) { }
}
