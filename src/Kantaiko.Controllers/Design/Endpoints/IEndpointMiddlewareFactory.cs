using Kantaiko.Controllers.Middleware;

namespace Kantaiko.Controllers.Design.Endpoints
{
    public interface IEndpointMiddlewareFactory<TContext>
    {
        IEndpointMiddleware<TContext> CreateEndpointMiddleware(EndpointDesignContext context);
    }
}
