using System.Reflection;
using Kantaiko.Controllers.Introspection.Deconstruction;

namespace Kantaiko.Controllers;

/// <summary>
/// The attribute that is used to mark a parameter as a composite parameter.
/// <br/>
/// The properties of such parameter will be used as individual parameters for the endpoint.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class CompositeParameterAttribute : Attribute, IDeconstructionValidator
{
    bool IDeconstructionValidator.CanDeconstruct(Type type, ICustomAttributeProvider attributeProvider) => true;
}
