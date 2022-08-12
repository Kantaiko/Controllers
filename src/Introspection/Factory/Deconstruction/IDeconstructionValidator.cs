using System;
using System.Reflection;

namespace Kantaiko.Controllers.Introspection.Factory.Deconstruction;

public interface IDeconstructionValidator
{
    bool CanDeconstruct(Type type, ICustomAttributeProvider attributeProvider);
}
