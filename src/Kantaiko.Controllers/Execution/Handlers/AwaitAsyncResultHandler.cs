using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution.Handlers;

public class AwaitAsyncResultHandler<TContext> : ControllerExecutionHandler<TContext>
{
    private readonly ConcurrentDictionary<Type, Func<object, object>> _resultAccessorCache = new();

    protected override async Task<ControllerResult> HandleAsync(ControllerContext<TContext> context, NextAction next)
    {
        if (context.RawResult is Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                return ControllerResult.Exception(exception);
            }

            var resultAccessor = _resultAccessorCache.GetOrAdd(task.GetType(), CreateResultAccessor);

            context.Result = resultAccessor(task);
        }
        else
        {
            context.Result = context.RawResult;
        }

        return await next();
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
