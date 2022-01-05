using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.Matching;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface IEndpointMatcherFactory<TContext> where TContext : IContext
{
    IEndpointMatcher<TContext> CreateEndpointMatcher(EndpointFactoryContext context);
}
