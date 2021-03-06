using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;
using Kantaiko.Routing.Abstractions;

namespace Kantaiko.Controllers.Execution.Handlers;

public class InstantiateControllerHandler<TContext> : ControllerExecutionHandler<TContext>
{
    private readonly IHandlerFactory _handlerFactory;

    public InstantiateControllerHandler(IHandlerFactory? handlerFactory = null)
    {
        _handlerFactory = handlerFactory ?? DefaultHandlerFactory.Instance;
    }

    protected override Task<ControllerResult> HandleAsync(ControllerContext<TContext> context, NextAction next)
    {
        PropertyNullException.ThrowIfNull(context.Endpoint);
        PropertyNullException.ThrowIfNull(context.Endpoint.Controller);

        context.ControllerInstance = _handlerFactory
            .CreateHandler(context.Endpoint.Controller.Type, context.ServiceProvider);

        if (context.ControllerInstance is IContextAcceptor<TContext> contextAcceptor)
        {
            contextAcceptor.SetContext(context.RequestContext);
        }

        return next();
    }
}
