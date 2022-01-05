using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution.Handlers;

public class ConvertParametersHandler<TContext> : ControllerExecutionHandler<TContext>
    where TContext : IContext
{
    private readonly IEnumerable<IHandler<ParameterConversionContext<TContext>, Task<Unit>>> _handlers;

    public ConvertParametersHandler(
        IEnumerable<IHandler<ParameterConversionContext<TContext>, Task<Unit>>> handlers)
    {
        _handlers = handlers;
    }

    protected override async Task<ControllerExecutionResult> HandleAsync(ControllerExecutionContext<TContext> context,
        NextAction next)
    {
        PropertyNullException.ThrowIfNull(context.Endpoint);

        var conversionContexts = context.Endpoint.Parameters
            .Select(x => new ParameterConversionContext<TContext>(context, x))
            .ToArray();

        context.ResolvedParameters = new Dictionary<EndpointParameterInfo, object?>(conversionContexts.Length);

        foreach (var handler in _handlers)
        {
            foreach (var conversionContext in conversionContexts)
            {
                await handler.Handle(conversionContext);

                if (conversionContext.ExecutionResult is not null)
                {
                    return conversionContext.ExecutionResult;
                }

                if (conversionContext.ValueResolved)
                {
                    context.ResolvedParameters[conversionContext.Parameter] = conversionContext.ResolvedValue;
                }
            }
        }

        return await next();
    }
}
