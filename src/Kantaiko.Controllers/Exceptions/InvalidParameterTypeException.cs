using System;
using System.Collections.Generic;
using System.Linq;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Exceptions
{
    public class InvalidParameterTypeException : ControllersException
    {
        public InvalidParameterTypeException(EndpointParameterInfo parameterInfo,
            IReadOnlyList<Type> allowedTypes) : base(
            $"The type \"{parameterInfo.ParameterType.Name}\" of parameter \"{parameterInfo.Name}\" is invalid. " +
            $"Allowed types: {string.Join(", ", allowedTypes.Select(x => x.Name))}") { }
    }
}
