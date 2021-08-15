using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Exceptions
{
    public class ServiceNotFoundException : ControllersException
    {
        public ServiceNotFoundException(EndpointParameterInfo parameterInfo) : base(
            $"Service with type \"{parameterInfo.ParameterType}\" was not registered, " +
            $"but is required by endpoint \"{parameterInfo.Endpoint.MethodInfo.Name}\"") { }
    }
}
