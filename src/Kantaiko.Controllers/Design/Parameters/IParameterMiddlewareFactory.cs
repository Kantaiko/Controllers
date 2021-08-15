using Kantaiko.Controllers.Middleware;

namespace Kantaiko.Controllers.Design.Parameters
{
    public interface IParameterMiddlewareFactory<TContext>
    {
        IEndpointMiddleware<TContext> CreateParameterMiddleware(EndpointParameterDesignContext context);
    }
}
