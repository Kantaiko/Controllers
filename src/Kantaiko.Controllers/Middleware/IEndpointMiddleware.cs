using System.Threading;
using System.Threading.Tasks;

namespace Kantaiko.Controllers.Middleware
{
    public interface IEndpointMiddleware<TContext>
    {
        EndpointMiddlewareStage Stage { get; }

        Task HandleAsync(EndpointMiddlewareContext<TContext> context, CancellationToken cancellationToken);
    }
}
