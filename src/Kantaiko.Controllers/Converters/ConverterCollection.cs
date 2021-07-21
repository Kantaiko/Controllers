using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Utils;

namespace Kantaiko.Controllers.Converters
{
    public class ConverterCollection : IConverterCollection
    {
        public IReadOnlyDictionary<Type, Type> ConverterTypes { get; }

        public ConverterCollection(IEnumerable<Type> converterTypes)
        {
            var assemblies = new[] {typeof(ConverterCollection).Assembly};
            var defaultTypes = ReflectionUtils.GetDerivedTypesFromAssemblies<IAutoRegistrableConverter>(assemblies);

            ConverterTypes = converterTypes.Concat(defaultTypes)
                .Distinct()
                .Where(ReflectionUtils.IsAutoRegistrable<IAutoRegistrableConverter>)
                .ToDictionary(k =>
                {
                    var converterInterface = ReflectionUtils.GetInheritedGenericInterface(k,
                        typeof(IAutoRegistrableConverter<>));

                    Debug.Assert(converterInterface is not null);

                    var genericArguments = converterInterface.GetGenericArguments();
                    Debug.Assert(genericArguments.Length == 1);

                    return genericArguments[0];
                }, v => v);
        }

        public static ConverterCollection FromAssemblies(params Assembly[] assemblies)
        {
            var converterTypes = ReflectionUtils.GetDerivedTypesFromAssemblies<IAutoRegistrableConverter>(assemblies);
            return new ConverterCollection(converterTypes);
        }

        public Type ResolveConverterType(Type parameterType)
        {
            return ConverterTypes.TryGetValue(parameterType, out var value)
                ? value
                : throw new ConverterNotFoundException(parameterType);
        }

        public static ConverterCollection Default { get; } = new(ArraySegment<Type>.Empty);
    }
}
