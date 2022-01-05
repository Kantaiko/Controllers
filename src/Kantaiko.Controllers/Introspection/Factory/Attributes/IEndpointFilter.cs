using Kantaiko.Controllers.Introspection.Factory.Context;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface IEndpointFilter
{
    bool IsIncluded(EndpointFactoryContext context);
}
