using System.Threading;
using System.Threading.Tasks;

namespace Kantaiko.Controllers.Middleware
{
    public abstract class EndpointMiddleware<TRequest> : IEndpointMiddleware<TRequest>, IAutoRegistrableMiddleware
    {
        public abstract EndpointMiddlewareStage Stage { get; }

        public abstract Task HandleAsync(EndpointMiddlewareContext<TRequest> context,
            CancellationToken cancellationToken);
    }
}
