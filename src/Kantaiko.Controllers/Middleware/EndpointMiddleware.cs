using System.Threading;
using System.Threading.Tasks;

namespace Kantaiko.Controllers.Middleware
{
    public abstract class EndpointMiddleware<TContext> : IEndpointMiddleware<TContext>,
        IAutoRegistrableMiddleware<TContext>
    {
        public abstract EndpointMiddlewareStage Stage { get; }

        public abstract Task HandleAsync(EndpointMiddlewareContext<TContext> context,
            CancellationToken cancellationToken);
    }
}
