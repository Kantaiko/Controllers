using System;
using System.Linq;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Validation
{
    public static class ParameterHelper
    {
        public static void CheckType(EndpointParameterInfo parameterInfo, params Type[] allowedTypes)
        {
            if (!allowedTypes.Contains(parameterInfo.ParameterType))
                throw new InvalidParameterTypeException(parameterInfo, allowedTypes);
        }
    }
}
