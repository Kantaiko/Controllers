using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Kantaiko.Controllers.Utils
{
    internal static class ReflectionUtils
    {
        public static IEnumerable<Type> GetDerivedTypesFromAssemblies<T>(IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(type => type.IsAssignableTo(typeof(T)) && type != typeof(T)));
        }

        public static bool IsAutoRegistrable<TMarker>(Type type) =>
            type.IsClass && !type.IsAbstract && type.IsAssignableTo(typeof(TMarker));

        private static Type? GetInheritedInterfaces(Type type, Type genericDefinition)
        {
            foreach (var typeInterface in type.GetInterfaces())
            {
                if (typeInterface.IsGenericType && typeInterface.GetGenericTypeDefinition() == genericDefinition)
                    return typeInterface;

                var interfaceResult = GetInheritedInterfaces(typeInterface, genericDefinition);
                if (interfaceResult is not null) return interfaceResult;
            }

            return type.BaseType is null ? null : GetInheritedInterfaces(type.BaseType, genericDefinition);
        }

        public static Type? GetInheritedGenericInterface(Type type, Type genericDefinition)
        {
            Debug.Assert(genericDefinition.IsInterface);
            Debug.Assert(genericDefinition.IsGenericTypeDefinition);

            return GetInheritedInterfaces(type, genericDefinition);
        }
    }
}
