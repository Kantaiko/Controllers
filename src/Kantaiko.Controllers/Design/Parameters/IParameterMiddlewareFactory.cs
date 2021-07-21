using Kantaiko.Controllers.Middleware;

namespace Kantaiko.Controllers.Design.Parameters
{
    public interface IParameterMiddlewareFactory<TRequest>
    {
        IEndpointMiddleware<TRequest> CreateParameterMiddleware(EndpointParameterDesignContext context);
    }
}
