using System;
using System.Linq;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.ParameterConversion.Exceptions;

namespace Kantaiko.Controllers.ParameterConversion;

public static class ParameterHelper
{
    public static void CheckType(EndpointParameterInfo parameterInfo, params Type[] allowedTypes)
    {
        if (!allowedTypes.Contains(parameterInfo.ParameterType))
            throw new InvalidParameterTypeException(parameterInfo, allowedTypes);
    }
}
