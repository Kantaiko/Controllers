using System.Threading.Tasks;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution.Handlers;

public class CompleteRequestHandler<TContext> : ControllerExecutionHandler<TContext> where TContext : IContext
{
    protected override Task<ControllerExecutionResult> HandleAsync(ControllerExecutionContext<TContext> context,
        NextAction next)
    {
        return Task.FromResult(ControllerExecutionResult.Success(context.Result ?? context.RawResult));
    }
}
