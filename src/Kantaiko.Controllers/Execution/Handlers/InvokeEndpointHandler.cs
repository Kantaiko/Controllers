using System.Reflection;
using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution.Handlers;

public class InvokeEndpointHandler<TContext> : IControllerExecutionHandler<TContext>
{
    public Task HandleAsync(ControllerExecutionContext<TContext> context)
    {
        PropertyNullException.ThrowIfNull(context.Endpoint);
        PropertyNullException.ThrowIfNull(context.ControllerInstance);

        try
        {
            context.RawInvocationResult = context.Endpoint.MethodInfo
                .Invoke(context.ControllerInstance, context.ConstructedParameters);
        }
        catch (TargetInvocationException exception)
        {
            context.ExecutionResult = ControllerExecutionResult.Exception(exception.InnerException!);
        }

        return Task.CompletedTask;
    }
}
