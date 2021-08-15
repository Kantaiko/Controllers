using Kantaiko.Controllers.Matchers;

namespace Kantaiko.Controllers.Design.Endpoints
{
    public interface IEndpointMatcherFactory<TContext>
    {
        IEndpointMatcher<TContext> CreateEndpointMatcher(EndpointDesignContext context);
    }
}
