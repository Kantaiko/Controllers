using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kantaiko.Controllers.Utils;

namespace Kantaiko.Controllers.Middleware
{
    public class MiddlewareCollection : IMiddlewareCollection
    {
        public IReadOnlyList<Type> MiddlewareTypes { get; }

        public MiddlewareCollection(IEnumerable<Type> middlewareTypes)
        {
            MiddlewareTypes = middlewareTypes.Distinct()
                .Where(ReflectionUtils.IsAutoRegistrable<IAutoRegistrableMiddleware>)
                .ToArray();
        }

        public static MiddlewareCollection FromAssemblies(params Assembly[] assemblies)
        {
            var converterTypes = ReflectionUtils.GetDerivedTypesFromAssemblies<IAutoRegistrableMiddleware>(assemblies);
            return new MiddlewareCollection(converterTypes);
        }

        public static MiddlewareCollection Empty { get; } = new(ArraySegment<Type>.Empty);
    }
}
