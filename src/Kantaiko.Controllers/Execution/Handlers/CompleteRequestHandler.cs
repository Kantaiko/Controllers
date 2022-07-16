using System.Threading.Tasks;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution.Handlers;

public class CompleteRequestHandler<TContext> : ControllerExecutionHandler<TContext>
{
    protected override Task<ControllerResult> HandleAsync(ControllerContext<TContext> context, NextAction next)
    {
        return Task.FromResult(ControllerResult.Success(context.Result ?? context.RawResult));
    }
}
