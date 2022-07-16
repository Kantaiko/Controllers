using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Execution.Properties;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution.Handlers;

public class ExecuteControllerSubHandlersHandler<TContext> : ControllerExecutionHandler<TContext>
{
    protected override Task<ControllerResult> HandleAsync(ControllerContext<TContext> context, NextAction next)
    {
        PropertyNullException.ThrowIfNull(context.Endpoint?.Controller);

        if (ControllerExecutionProperties<TContext>.Of(context.Endpoint.Controller)?.Handler is { } handler)
        {
            return handler.Handle(context, () => next());
        }

        return next();
    }
}
