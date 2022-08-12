using System;
using Kantaiko.Controllers.Introspection.Factory.Context;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface ITextParameterConverterTypeProvider
{
    Type GetTextParameterConverterType(ParameterFactoryContext context);
}
