using System.Reflection;
using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution.Handlers;

public class InvokeEndpointHandler<TContext> : ControllerExecutionHandler<TContext> where TContext : IContext
{
    protected override async Task<ControllerExecutionResult> HandleAsync(ControllerExecutionContext<TContext> context,
        NextAction next)
    {
        PropertyNullException.ThrowIfNull(context.Endpoint);
        PropertyNullException.ThrowIfNull(context.ControllerInstance);

        try
        {
            context.RawResult = context.Endpoint.MethodInfo
                .Invoke(context.ControllerInstance, context.ConstructedParameters);
        }
        catch (TargetInvocationException exception)
        {
            return ControllerExecutionResult.Exception(exception.InnerException!);
        }

        return await next();
    }
}
