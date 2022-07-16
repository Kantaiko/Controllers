using System;
using System.Threading.Tasks;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution.Handlers;

public abstract class ControllerExecutionHandler<TContext> : IControllerExecutionHandler<TContext>
{
    protected abstract Task<ControllerResult> HandleAsync(ControllerContext<TContext> context, NextAction next);

    protected delegate Task<ControllerResult> NextAction();

    public Task<ControllerResult> Handle(ControllerContext<TContext> input,
        Func<ControllerContext<TContext>, Task<ControllerResult>> next)
    {
        return HandleAsync(input, () => next(input));
    }
}
