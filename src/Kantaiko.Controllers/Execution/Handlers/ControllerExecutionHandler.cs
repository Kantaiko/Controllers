using System;
using System.Threading.Tasks;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution.Handlers;

public abstract class ControllerExecutionHandler<TContext> :
    IChainedHandler<ControllerExecutionContext<TContext>, Task<ControllerExecutionResult>>
    where TContext : IContext
{
    protected abstract Task<ControllerExecutionResult> HandleAsync(ControllerExecutionContext<TContext> context,
        NextAction next);

    protected delegate Task<ControllerExecutionResult> NextAction(ControllerExecutionContext<TContext>? context = null);

    public Task<ControllerExecutionResult> Handle(ControllerExecutionContext<TContext> input,
        Func<ControllerExecutionContext<TContext>, Task<ControllerExecutionResult>> next)
    {
        return HandleAsync(input, context => next(context ?? input));
    }
}
