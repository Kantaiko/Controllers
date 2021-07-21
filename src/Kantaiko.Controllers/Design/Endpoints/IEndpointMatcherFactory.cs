using Kantaiko.Controllers.Matchers;

namespace Kantaiko.Controllers.Design.Endpoints
{
    public interface IEndpointMatcherFactory<TRequest>
    {
        IEndpointMatcher<TRequest> CreateEndpointMatcher(EndpointDesignContext context);
    }
}
