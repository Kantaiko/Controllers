using System.Threading.Tasks;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution.Handlers;

public class CompleteRequestHandler<TContext> : IControllerExecutionHandler<TContext>
{
    public Task HandleAsync(ControllerExecutionContext<TContext> context)
    {
        context.ExecutionResult ??= ControllerExecutionResult.Success(
            context.InvocationResult ?? context.RawInvocationResult);

        return Task.CompletedTask;
    }
}
