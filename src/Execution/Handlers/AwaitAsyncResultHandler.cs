using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution.Handlers;

public class AwaitAsyncResultHandler<TContext> : IControllerExecutionHandler<TContext>
{
    private readonly ConcurrentDictionary<Type, Func<object, object>> _resultAccessorCache = new();

    public async Task HandleAsync(ControllerExecutionContext<TContext> context)
    {
        if (context.RawInvocationResult is Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                context.ExecutionResult = ControllerExecutionResult.Exception(exception);
                return;
            }

            var resultAccessor = _resultAccessorCache.GetOrAdd(task.GetType(), CreateResultAccessor);

            context.InvocationResult = resultAccessor(task);
        }
        else
        {
            context.InvocationResult = context.RawInvocationResult;
        }
    }

    private static Func<object, object> CreateResultAccessor(Type type)
    {
        var parameter = Expression.Parameter(typeof(object));
        var taskLike = Expression.Convert(parameter, type);
        var expression = Expression.Property(taskLike, "Result");
        var result = Expression.Convert(expression, typeof(object));

        return Expression.Lambda<Func<object, object>>(result, parameter).Compile();
    }
}
