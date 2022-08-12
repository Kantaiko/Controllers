using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Handlers;

namespace Kantaiko.Controllers.Execution.Handlers;

public class InstantiateControllerHandler<TContext> : IControllerExecutionHandler<TContext>
{
    private readonly IHandlerFactory _handlerFactory;

    public InstantiateControllerHandler(IHandlerFactory? handlerFactory = null)
    {
        _handlerFactory = handlerFactory ?? DefaultHandlerFactory.Instance;
    }

    public Task HandleAsync(ControllerExecutionContext<TContext> context)
    {
        PropertyNullException.ThrowIfNull(context.Endpoint);
        PropertyNullException.ThrowIfNull(context.Endpoint.Controller);

        context.ControllerInstance = _handlerFactory
            .CreateHandler(context.Endpoint.Controller.Type, context.ServiceProvider);

        if (context.ControllerInstance is IContextAcceptor<TContext> contextAcceptor)
        {
            contextAcceptor.SetContext(context.RequestContext);
        }

        return Task.CompletedTask;
    }
}
