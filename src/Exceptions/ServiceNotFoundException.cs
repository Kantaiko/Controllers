using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.Exceptions;

/// <summary>
/// The exception that is thrown when a service cannot be found.
/// </summary>
public sealed class ServiceNotFoundException : Exception
{
    internal ServiceNotFoundException(EndpointParameterInfo parameterInfo) : base(
        string.Format(Strings.ServiceNotFound,
            parameterInfo.ParameterType,
            parameterInfo.Endpoint.MethodInfo.Name)
    ) { }
}
