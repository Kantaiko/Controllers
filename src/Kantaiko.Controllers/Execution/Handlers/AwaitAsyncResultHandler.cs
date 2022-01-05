using System;
using System.Threading.Tasks;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution.Handlers;

public class AwaitAsyncResultHandler<TContext> : ControllerExecutionHandler<TContext> where TContext : IContext
{
    protected override async Task<ControllerExecutionResult> HandleAsync(ControllerExecutionContext<TContext> context,
        NextAction next)
    {
        if (context.RawResult is Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                return ControllerExecutionResult.Exception(exception);
            }

            var resultProperty = task.GetType().GetProperty("Result");
            context.Result = resultProperty!.GetValue(task);
        }

        return await next();
    }
}
