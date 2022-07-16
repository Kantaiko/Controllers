using System.Reflection;
using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution.Handlers;

public class InvokeEndpointHandler<TContext> : ControllerExecutionHandler<TContext>
{
    protected override async Task<ControllerResult> HandleAsync(ControllerContext<TContext> context, NextAction next)
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
            return ControllerResult.Exception(exception.InnerException!);
        }

        return await next();
    }
}
