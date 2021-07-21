using System.Threading;
using System.Threading.Tasks;

namespace Kantaiko.Controllers.Middleware
{
    public interface IEndpointMiddleware<TRequest>
    {
        EndpointMiddlewareStage Stage { get; }

        Task HandleAsync(EndpointMiddlewareContext<TRequest> context, CancellationToken cancellationToken);
    }
}
