using Kantaiko.Controllers.Middleware;

namespace Kantaiko.Controllers.Design.Endpoints
{
    public interface IEndpointMiddlewareFactory<TRequest>
    {
        IEndpointMiddleware<TRequest> CreateEndpointMiddleware(EndpointDesignContext context);
    }
}
