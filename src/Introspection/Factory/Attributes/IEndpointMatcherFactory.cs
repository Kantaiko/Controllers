using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.Matching;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface IEndpointMatcherFactory<TContext>
{
    IEndpointMatcher<TContext> CreateEndpointMatcher(EndpointFactoryContext context);
}
