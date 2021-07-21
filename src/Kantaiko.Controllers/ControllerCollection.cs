using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kantaiko.Controllers.Internal;
using Kantaiko.Controllers.Utils;

namespace Kantaiko.Controllers
{
    public class ControllerCollection : IControllerCollection
    {
        public IReadOnlyList<Type> ControllerTypes { get; }

        public ControllerCollection(IEnumerable<Type> controllerTypes)
        {
            ControllerTypes = controllerTypes.Distinct()
                .Where(ReflectionUtils.IsAutoRegistrable<IRequestAcceptor>)
                .ToArray();
        }

        public static ControllerCollection FromAssemblies(params Assembly[] assemblies)
        {
            var controllerTypes = ReflectionUtils.GetDerivedTypesFromAssemblies<IRequestAcceptor>(assemblies);
            return new ControllerCollection(controllerTypes);
        }
    }
}
