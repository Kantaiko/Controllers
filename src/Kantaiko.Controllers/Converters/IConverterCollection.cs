using System;
using System.Collections.Generic;

namespace Kantaiko.Controllers.Converters
{
    public interface IConverterCollection
    {
        IReadOnlyDictionary<Type, Type> ConverterTypes { get; }
        Type ResolveConverterType(Type parameterType);
    }
}
