using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Execution.Properties;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution.Handlers;

public class ExecuteControllerSubHandlersHandler<TContext> : ControllerExecutionHandler<TContext>
    where TContext : IContext
{
    protected override Task<ControllerExecutionResult> HandleAsync(ControllerExecutionContext<TContext> context,
        NextAction next)
    {
        PropertyNullException.ThrowIfNull(context.Endpoint?.Controller);

        if (ControllerExecutionProperties<TContext>.Of(context.Endpoint.Controller)?.Handler is { } handler)
        {
            return handler.Handle(context, x => next(x));
        }

        return next();
    }
}
