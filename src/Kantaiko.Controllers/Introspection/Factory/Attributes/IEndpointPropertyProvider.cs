using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface IEndpointPropertyProvider
{
    IImmutablePropertyCollection UpdateEndpointProperties(EndpointFactoryContext context);
}
