using System;

namespace Kantaiko.Controllers.Introspection.Factory.Deconstruction;

public interface IDeconstructionValidator
{
    bool CanDeconstruct(Type type);
}
