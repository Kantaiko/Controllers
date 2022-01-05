using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Matching;

public interface IEndpointMatcher<TContext> where TContext : IContext
{
    EndpointMatchResult Match(EndpointMatchContext<TContext> context);
}
